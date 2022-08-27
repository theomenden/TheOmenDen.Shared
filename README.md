# The Omen Den Shared Library [![Build](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml)[![GitHub issues](https://img.shields.io/github/issues/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/issues)[![GitHub license](https://img.shields.io/github/license/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/blob/master/LICENSE.txt)[![GitHub stars](https://img.shields.io/github/stars/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/stargazers)

- ##  This is just a grouping of common classes that are used within every application that The Omen Den aims to provide as a company, and is free to modify, redistribute, and use elsewhere
 - ## Especially since this library is sure to not be unique to us.
 - ### Attributions for credits on each release are defined below
<br /><br />
# Goals that we aim for:
1. Relevant extensions across The Omen Den's software applications
2. Exception calling, and custom exceptions
3. Pooling extensions for StringBuilder and Arrays
   - Hopefully working towards far less overhead than constantly newing these types up
   - In the case of arrays, adding "slicing" SubArray functionality
4. Custom Enumerations
   - Providing ways to grade exceptions via gravity
   - Providing ways to use "smarter" enumerations for differing control flow
   - Provides two structs `Conditions and Consequences` that can be chained together against the `EnumerationBase` implementations
   - Inspired by:
     1. [The Smart Enumerations Library](https://github.com/ardalis/SmartEnum)
     2. [This article by Jimmy Bogard](https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/)
5. Provides simple LINQ methods for `ReadOnlySpan<T>`
6. Base Record types for `QueryStrings`
7. AsyncLazyInitializer type sourced from: [Microsoft Dev Blogs](https://devblogs.microsoft.com/pfxteam/asynclazyt/)
8. Extensions on the `Type Type` to allow for easier retrieval of ancestors
9. Provides `Progress Bars` that can also be threadsafe, as well as a simple progress indicator.
   - Gives the caller the ability to specify a change in the `ConsoleColor` with any one of the provided colors.
   - Provides a relatively smooth, and rate-adjustable animation on the progress indicator
10. Provides `Guard` methods that can be used to help control the flow and validtion of models and creation of various components in an application domain.
    - Inspired By:
      1. [Guard.Net](https://github.com/george-pancescu/Guard)
      2. [This video by Derek Comartin of Code Opinion](https://youtu.be/9cr7grNWn6c)
      3. [This article by Stavros Kasidis](https://stavroskasidis.com/blog/2017/tips-and-tricks-1-guard-clauses/)

# Async Stream Handling Features
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

# EnumerationBase
1. These `Record Types` are implemented as such: 
   - `EnumerationBase<TKey>`
     - Similar to the standard Enumeration `Enum` type that you will see in `C#` but provides the ability to work with Condtions (When) and Consequences (Then) allowing for the expansion of logic ontop of Enumerations.
     - We provide a way to customize this further by using `EnumrationBase<TKey, TEnumeration>` to define a customized "backing field" for your own design.
     - We also provide guards, and relevant exceptions to assist in runtime "Early Swift Failure" practices. 
   - `EnumerationFlagBase<TKey>`
     -  This record allows for your enumerations to be extended to have a similar functionality as working with bit flags on your standard `C#` enumerations.
     -  This is further enhanced by the use of our `Condtions (When) ... Consequences (Then)` pattern - allowing for more robust and well defined logic.
     -  Strictly enforced by makign the definition of the enum require an integer backing field using powers of two (2<sup>n</sup>)
        1. 2,4,8,16... 2<sup>n</sup>

# Guards   
1. We have create a `Guard` partial class system
   - Our aim/intention with this is to allow for a "fail early" strategy at your domain's edge.
   - We encourage you **NOT** to use these classes within your business logic/application domain since that seems to be an anti-pattern.
   - There are also differing typed exceptions that are thrown per condition, as well as provided exception message templates. 
   - If you desire to define your own custom Guard - You would want to work with the `Guard.FromCondition` method. This allows for the passing of a delegate that has a boolean result, and an `Exception` object as an outcome.
     1. e.g. `Guard.FromCondition(Func<Boolean>, Exception exception)` 

# TODOs:
1. Provide better source generation for our generic implementations
2. Provide clearer, and more accessible documentation
3. Add `.NET 7` and `.NET Standard` build targets.
4. General clean-up, and optimizations


