# The Omen Den Shared Library [![Build](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/theomenden/TheOmenDen.Shared/actions/workflows/sonarcloud.yml)[![GitHub issues](https://img.shields.io/github/issues/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/issues)[![GitHub license](https://img.shields.io/github/license/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/blob/master/LICENSE.txt)[![GitHub stars](https://img.shields.io/github/stars/theomenden/TheOmenDen.Shared?style=plastic)](https://github.com/theomenden/TheOmenDen.Shared/stargazers)

- ###  This is just a grouping of common classes that are used within every application that The Omen Den aims to provide as a company, and is free to modify, redistribute, and use elsewhere
 - Especially since this library is sure to not be unique to us.
 -  Attributions for credits on each release are defined below
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
1. We have created a `Guard` partial class system
   - Our aim/intention with this is to allow for a "fail early" strategy at your domain's edge.
   - We encourage you **NOT** to use these classes within your business logic/application domain since that seems to be an anti-pattern.
   - There are also differing typed exceptions that are thrown per condition, as well as provided exception message templates. 
   - If you desire to define your own custom Guard - You would want to work with the `Guard.FromCondition` method. This allows for the passing of a delegate that has a boolean result, and an `Exception` object as an outcome.
     1. e.g. `Guard.FromCondition(Func<Boolean>, Exception exception)` 

# Random Generations
1. ~~The need for generating random values across threads has been shown for us at The Omen Den - especially as we work on [Crows Against Humility](https://crowsagainsthumility.app)~~
    - ~~Our aim/intention was to provide a thread safe implementation of .NET's `Random`.~~
    - ~~We of course will credit Alexey from [this stack overflow] post for the basis of our design and documentation - it is important to continue to recognize where our knowledge comes from.~~
    - ~~`ThreadSafeRandom` implements both `IDisposable` and `IAsyncDisposable` to allow for the destruction/freeing of resources used by `ThreadLocal`~~
    - This was replaced with the .NET6 implementation of `System.Random.Shared`, a thread-safe Random Number Generator.
2. We implemented a shuffle algorithm, the [Fisher-Yates-Durstenfeld Shuffle algorithm](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm)

- ### A few Benchmarks, as generated by Benchmark.NET (smallest 10,small 100, medium 1000, large 1,000,000 integers)
 
 #### Below Dataset is for when a `List<Int32>` is supplied
|                               Method |              Mean |           Error |          StdDev | Completed Work Items | Lock Contentions |     Gen0 |     Gen1 |     Gen2 | Allocated |
|------------------------------------- |------------------:|----------------:|----------------:|---------------------:|-----------------:|---------:|---------:|---------:|----------:|
|  GetRandomElementFromSmallestDataSet |         16.827 ns |       0.1809 ns |       0.1413 ns |                    - |                - |        - |        - |        - |         - |
|     GetRandomElementFromSmallDataSet |         12.729 ns |       0.1129 ns |       0.1001 ns |                    - |                - |        - |        - |        - |         - |
|    GetRandomElementFromMediumDataSet |          9.705 ns |       0.0698 ns |       0.0619 ns |                    - |                - |        - |        - |        - |         - |
|     GetRandomElementFromLargeDataSet |         18.914 ns |       0.3749 ns |       0.4167 ns |                    - |                - |        - |        - |        - |         - |
| GetRandomElementsFromSmallestDataSet |        191.513 ns |       1.9762 ns |       1.8485 ns |                    - |                - |   0.0212 |        - |        - |     224 B |
|    GetRandomElementsFromSmallDataSet |      1,386.329 ns |       9.6764 ns |       9.0513 ns |                    - |                - |   0.0896 |        - |        - |     944 B |
|   GetRandomElementsFromMediumDataSet |     12,059.781 ns |     111.0186 ns |      98.4151 ns |                    - |                - |   0.7782 |        - |        - |    8144 B |
|    GetRandomElementsFromLargeDataSet | 17,467,272.141 ns | 349,145.9364 ns | 680,982.3344 ns |                    - |                - | 562.5000 | 562.5000 | 562.5000 | 8000335 B |

#### Dataset is for when an `Int32[]` is supplied
|                               Method |             Mean |          Error |         StdDev | Completed Work Items | Lock Contentions |     Gen0 |     Gen1 |     Gen2 | Allocated |
|------------------------------------- |-----------------:|---------------:|---------------:|---------------------:|-----------------:|---------:|---------:|---------:|----------:|
|  GetRandomElementFromSmallestDataSet |         22.77 ns |       0.350 ns |       0.327 ns |                    - |                - |        - |        - |        - |         - |
|     GetRandomElementFromSmallDataSet |         15.08 ns |       0.115 ns |       0.096 ns |                    - |                - |        - |        - |        - |         - |
|    GetRandomElementFromMediumDataSet |         10.98 ns |       0.045 ns |       0.037 ns |                    - |                - |        - |        - |        - |         - |
|     GetRandomElementFromLargeDataSet |         16.23 ns |       0.253 ns |       0.237 ns |                    - |                - |        - |        - |        - |         - |
| GetRandomElementsFromSmallestDataSet |        160.83 ns |       1.506 ns |       1.335 ns |                    - |                - |   0.0153 |        - |        - |     160 B |
|    GetRandomElementsFromSmallDataSet |      1,320.57 ns |       5.449 ns |       4.550 ns |                    - |                - |   0.0496 |        - |        - |     520 B |
|   GetRandomElementsFromMediumDataSet |     12,263.08 ns |      72.492 ns |      60.534 ns |                    - |                - |   0.3815 |        - |        - |    4120 B |
|    GetRandomElementsFromLargeDataSet | 14,419,147.21 ns | 255,304.617 ns | 322,878.596 ns |                    - |                - | 328.1250 | 328.1250 | 328.1250 | 4000230 B |

## Model Contracts
- `IUser`
  - A simple interface to mark the definition of a `user`.
    - `Id` - a GUID/UUID that allows for easy storage and retrieval of user information.
    - `Email` - an individual's email address.
    - `Name` - the name of the user.
    - `IsAuthenticated` - indicates whether the user is authenticated.
    - `Key` - the key associated with the user (integer based).
- `ITenant`
  - A representation of the container for organization/business items, logic, and information.
    - `Id` - a GUID/UUID that allows for easy storage and retrieval of tenant information.
    - `Code` - The code associated with the tenant.
    - `Name` - the name of the tenant.
    - `Key` - the key associated with the tenant (integer based).
- `IEntityKey`
  - Provides a generic interpretation to help distinguish entities from each other.
    - `Id` - a GUID/UUID that allows for easy storage and retrieval.
    - `CreatedAt` - a timestamp marking the entity's creation.
    - `ITenant` - The Tenant that entity is associated with.
    - `IUser` - the originator/creator of the entity.  
- `IEntity`
  - A simple marker interface that provides an implementation with a way to define a unique `IEntityKey` on an Entity.
    - `IEntityKey` the unique key associated with the entity.

## Specifications
 - We've implemented a basic specification pattern. The idea here is that it should be robust enough to use in a common case, but not too complicated that it will require a steep learning curve
 - We've added the following logical operations to the specification pattern
   - `AND` which combines two specifications and returns the combined result using the AND logical truth table 
   - `OR` which combines two specifications and returns the combined result using the OR logical truth table
   - `NOR` which combines two specifications and returns the combined result using the NOR logical truth table (Bubble And)
   - `NAND` which combines two specifications and returns the combined result using the NAND logical truth table (Bubble Or)
   - `NOT` which inverts the logical truth table for the provided specification.
 - These do use System.Linq.Expressions, and are implemented over record types, with the idea that a specification should be weighed in it's usage by the intent of the domain it belongs to. This also allows us to have a sort of better knowledge of thread safety and mitigate certain performance issues. We're hoping to continue to improve upon these metrics, and can provide unit test coverages, as well as further performance metrics.
 - An example of using this pattern can be found in the Unit Tests Repository, which is located at [TheOmenDen.Shared.Tests](https://github.com/theomenden/TheOmenDen.Shared.Tests).
# TODOs:
1. Provide better source generation for our generic implementations
2. Provide clearer, and more accessible documentation
3. General clean-up, and optimizations


