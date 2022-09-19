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
1. We have created a `Guard` partial class system
   - Our aim/intention with this is to allow for a "fail early" strategy at your domain's edge.
   - We encourage you **NOT** to use these classes within your business logic/application domain since that seems to be an anti-pattern.
   - There are also differing typed exceptions that are thrown per condition, as well as provided exception message templates. 
   - If you desire to define your own custom Guard - You would want to work with the `Guard.FromCondition` method. This allows for the passing of a delegate that has a boolean result, and an `Exception` object as an outcome.
     1. e.g. `Guard.FromCondition(Func<Boolean>, Exception exception)` 

# Random Generations
1. The need for generating random values across threads has been shown for us at The Omen Den - especially as we work on [Crows Against Humility](https://crowsagainsthumility.app)
    - Our aim/intention was to provide a thread safe implementation of .NET's `Random`.
    - We of course will credit Alexey from [this stack overflow] post for the basis of our design and documentation - it is important to continue to recognize where our knowledge comes from.
    - `ThreadSafeRandom` implements both `IDisposable` and `IAsyncDisposable` to allow for the destruction/freeing of resources used by `ThreadLocal`
    - We don't use a locking mechanism here, to avoid the complications and relative expense
    - However, we also recognized that by using a static implementation, there could be complications from "newing" up an instance
2. We have also incorporated this approach in our `Linq` implementations of random element retrieval. 
    - [This article](https://www.genericgamedev.com/general/random-elements-subsets-and-shuffling-collections-linq-style/) by [Paul Scharf](http://paulscharf.com/) provided enough basis and inspiration for our approach
    - With the ability to provide some basic optimizations for the following implementations: `ICollection<T>` `T[]` and `List<T>` - we were able to see linear performance, as well as reduced allocations
    - We also have the methods for Arrays (`T[]`) and Lists `List<T>` as separate methods with the types appended to their suffix. (e.g. `GetRandomElementFromArray`)
    - We split up our methods into two main ideas
      1. `GetRandomElement(s)` - To retrieve the actual `TValue` from the supplied source collection
      2. `GetRandomIndex(Indicies)`- To retrieve an `Index` struct for further usage from the supplied source collection.
    - We do also have benchmarks available, but we also believe that O(n) (mentioned in the above article) - was probably the most optimized we could get. The most optimizations we retrieved were from space saving methods

   - ### A few Benchmarks, as generated by Benchmark.NET (small array - 100 ints, medium array 100,000 ints, large array 1,000,000 ints)
 
| Method 							| Mean 		| Error 	    | StdDev 	  | Gen0   | Allocated |
|---------------------------------------------- |----------------:|--------------:|--------------:|-------:|----------:|
| GetRandomElementFromSmallArray 		      |        52.35 ns |      0.374 ns |      0.415 ns |      - |         - |
| GetRandomElementFromMediumArray		      |        43.78 ns |      0.625 ns |      0.585 ns |      - |         - |
| GetRandomElementFromLargeArray 		      |        46.94 ns |      0.658 ns |      0.856 ns |      - |         - |
| GetRandomElementFromSmallList  		      |        41.56 ns |      0.595 ns |      0.557 ns |      - |         - |
| GetRandomElementFromMediumList 		      |        31.98 ns |      0.631 ns |      0.559 ns |      - |         - |
| GetRandomElementFromLargeList  		      |        39.39 ns |      0.818 ns |      1.433 ns |      - |         - |
| GetSmallAmountOfRandomElementsBenchmarkArray  |       155.03 ns |      1.863 ns |      1.651 ns | 0.0076 |      48 B |
| GetMediumAmountOfRandomElementsBenchmarkArray |    22,297.34 ns |    246.435 ns |    192.401 ns |      - |      48 B |
| GetLargeAmountOfRandomElementsBenchmarkArray  | 2,314,022.18 ns | 23,111.151 ns | 20,487.433 ns |      - |      50 B |
|   GetSmallAmountOfRandomElementsBenchmarkList |       169.68 ns |      1.296 ns |      1.212 ns | 0.0076 |      48 B |
|  GetMediumAmountOfRandomElementsBenchmarkList |    23,649.32 ns |    407.402 ns |    361.151 ns |      - |      48 B |
|   GetLargeAmountOfRandomElementsBenchmarkList | 2,776,298.51 ns | 53,700.678 ns | 52,741.246 ns |      - |      50 B |
|       GetSmallAmountOfRandomElementsBenchmark |       239.36 ns |      3.045 ns |      2.700 ns | 0.0153 |      96 B |
|      GetMediumAmountOfRandomElementsBenchmark |    24,046.92 ns |    197.307 ns |    174.908 ns |      - |      96 B |
|       GetLargeAmountOfRandomElementsBenchmark | 2,854,750.20 ns | 27,621.984 ns | 24,486.170 ns |      - |      98 B |

<br/>
# TODOs:
1. Provide better source generation for our generic implementations
2. Provide clearer, and more accessible documentation
3. Add `.NET 7` and `.NET Standard` build targets.
4. General clean-up, and optimizations


