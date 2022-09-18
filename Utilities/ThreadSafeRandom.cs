namespace TheOmenDen.Shared.Utilities;

/// <summary>
/// <para>A possible way to solve .NET's lack of thread safe random number generation</para>
/// <para>By using <see cref="ThreadLocal{T}"/> and providing a static instance of a <seealso cref="Random" />
/// We aim to allow for parallel operations to have random numbers safely generated independent of each thread.
/// </para>
/// <para><see href="https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe">This StackOverflow for reference</see>
///  contains the original design by Alexey - as well as general justification for why this is implemented. Everything is sourced from them.
/// </para>
/// <inheritdoc cref="IDisposable"/>
/// <inheritdoc cref="IAsyncDisposable"/>
/// </summary>
/// <remarks>
/// Design notes
/// <list type="number">
/// <item>
/// Uses an instance of <see cref="Random" /> for each thread (provided by <see cref="ThreadLocal{T}"/>.
/// </item>
/// <item>
/// Seed can be set in ThreadSafeRandom constructor. Note: Be careful - one seed for all threads can lead same values for several threads.
/// </item>
/// <item>ThreadSafeRandom implements <see cref="Random"/> class for simple usage.</item>
/// <item>
/// ThreadSafeRandom can be used by global static instance. Example: <c>int randomInt = ThreadSafeRandom.Global.Next()</c>.
/// </item>
/// <item>
/// <see cref="IDisposable"/> to provide a cleanup mechanism at the end of the usage (using)
/// </item>
/// <item>
/// <see cref="IAsyncDisposable"/> to provide a cleanup mechanism across asynchronous usages ex. (await using)
/// </item>
/// </list>
/// </remarks>
public sealed class ThreadSafeRandom : Random, IDisposable, IAsyncDisposable
{
    private readonly ThreadLocal<Random> _threadLocalRandom;

    private bool _isDisposed;
    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="ThreadSafeRandom"/> class.
    /// </summary>
    /// <param name="seed">Optional seed for <see cref="Random"/>. If not provided then random seed will be used.</param>
    public ThreadSafeRandom(int? seed = null)
    {
        _threadLocalRandom = new (() => seed is not null 
            ? new Random(seed.Value) 
            : new Random());
    }
    #endregion
    #region Static Properties
    /// <summary>
    /// Gets global static instance.
    /// </summary>
    public static ThreadSafeRandom Global { get; } = new();

    /// <summary>
    /// Experimental workings with <see cref="Lazy{T}"/> - USE AT YOUR OWN RISK
    /// </summary>
    public static Lazy<ThreadSafeRandom> LazyRandom { get; } = new(() => Global);

    /// <summary>
    /// Experimental workings with <see cref="AsyncLazyInitializer{T}"/> - USE AT YOUR OWN RISK
    /// </summary>
    public static AsyncLazyInitializer<ThreadSafeRandom> AsyncLazyRandom { get; } = new(() => Global);
    #endregion
    #region Overrides
    /// <inheritdoc />
    public override int Next() => _threadLocalRandom.Value!.Next();

    /// <inheritdoc />
    public override int Next(int maxValue) => _threadLocalRandom.Value!.Next(maxValue);

    /// <inheritdoc />
    public override int Next(int minValue, int maxValue) => _threadLocalRandom.Value!.Next(minValue, maxValue);

    /// <inheritdoc />
    public override void NextBytes(byte[] buffer) => _threadLocalRandom.Value!.NextBytes(buffer);

    /// <inheritdoc />
    public override void NextBytes(Span<byte> buffer) => _threadLocalRandom.Value!.NextBytes(buffer);

    /// <inheritdoc />
    public override double NextDouble() => _threadLocalRandom.Value!.NextDouble();
    
    /// <inheritdoc />
    public override long NextInt64() => _threadLocalRandom.Value!.NextInt64();

    /// <inheritdoc />
    public override long NextInt64(long maxValue) => _threadLocalRandom.Value!.NextInt64(maxValue);

    /// <inheritdoc />
    public override long NextInt64(long minValue, long maxValue) =>
        _threadLocalRandom.Value!.NextInt64(minValue, maxValue);

    /// <inheritdoc />
    public override float NextSingle() => _threadLocalRandom.Value!.NextSingle();
    #endregion
    #region Destruction Implementations
    public ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return ValueTask.CompletedTask;
        }
        _threadLocalRandom.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _threadLocalRandom.Dispose();
        }

        _isDisposed = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
