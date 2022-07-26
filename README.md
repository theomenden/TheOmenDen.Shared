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
7. ~~Open Generic Registrations for IApiService implementations~~ Moved to TheOmenDen.Shared.Services
8. ~~Automatic Registrations for IApiService and IApiStreamService implementations via [Scrutor](https://github.com/khellang/Scrutor)~~ Moved to TheOmenDen.Shared.Services

## Moving from V1 -> V2 will incur breaking changes as I have determined that manipulating the shared libary as a massive project is a bit difficult to maintain. From now on, we'll keep the ideas of what each section of the shared project has separated. This is based off of necessity and my personal obession with cleanliness.

### Note: V2 will be identified by
   - Releases will tagged as V2
   - Package versions will have the first 3 parts of their versioning as a date after: `2022.7.26`