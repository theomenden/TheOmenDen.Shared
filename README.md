# The Omen Den Shared Library [![Build](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml)[![GitHub issues](https://img.shields.io/github/issues/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/issues)[![GitHub license](https://img.shields.io/github/license/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/blob/master/LICENSE.txt)[![GitHub stars](https://img.shields.io/github/stars/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/stargazers)

- This is just a grouping of common classes that are used within every application that The Omen Den aims to provide as a company, and is free to modify, redistribute, and use elsewhere
- Especially since this library is sure to not be unique to us.

## A grouping of classes that aims to provide:
1. Relevant extensions accross The Omen Den's software applications
2. Exception calling, and custom exceptions
3. Pooling extensions for Stringbuilder and Arrays
   - Hopefully working towards far less overhead than constantly newing these types up
   - In the case of arrays, adding "slicing" subarray functionality
4. Custom Enumerations (both standard, and "smart")
   - Providing ways to grade exceptions via gravity
   - Providing ways to use "smart" enumerations for differing control flow
5. Starting of LINQ methods for ReadOnlySpan<T>
   - Currently limited to a "lazy" implementation of `IEnumerable<T>.Any<T>()`;
6. Base Record types for QueryStrings and Events
   - Providing immutability

## Async Stream Handling Features
1. Provided in the `AsynchronousStreamOutcome` set of extensions are a few methods for capturing `OperationOutcome` during the iterations of an `IAsyncEnumerable<T>`
   - The `T` in question _must_ implement our `IEntity` interface. 
   - We aim to work solely with asynchronous iterations provided by an `await foreach()` pattern
   - We also aim to provide a simple, and easy to interpret API for further processing and alignment by avoiding an underlying `try...catch` within the Async Iterator.  
2. Our current implementation allows for the simplicity of just providing a long running delegate of an individual operation. `Func<in T, in CancellationToken, out ValueTask<OperationOutcome>>`
   - This guarantees that the underlying operation is asynchronous
   - This also ensures that the operation returns an `OperationOutcome` object 
3. With this provided, our intent on design is twofold -
   - Firstly We remove the basis for throwing an exception over the stream.
   - Secondly We allow for individual failures to occur during the stream.
     - With this approach, we aim to allow for multiple failures to occur during the stream, while maintaining a consistent reporting behavior to allow for a more streamlined client experience. 
     - We also aim for the `Tuple<T,OperationOutcome>` coupling to be a launching strategy for further processing.
    
### Note: V2 will be identified by
   - Releases will tagged as V2
   - Package versions will have the first 3 parts of their versioning as a date after: `2022.7.26`