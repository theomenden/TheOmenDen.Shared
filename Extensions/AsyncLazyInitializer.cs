namespace TheOmenDen.Shared.Extensions;
/// <summary>
/// Provides a way to lazily initialize normally asynchronous functions
/// </summary>
/// <typeparam name="T">The underlying type</typeparam>
/// <remarks>
/// This solution sourced from: <see href="https://devblogs.microsoft.com/pfxteam/asynclazyt/">Microsoft Dev Blogs</see>
/// </remarks>
public class AsyncLazyInitializer<T> : Lazy<Task<T>>
{
    public AsyncLazyInitializer(Func<T> valueFactory) :
        base(() => Task.Factory.StartNew(valueFactory))
    {}
    
    public AsyncLazyInitializer(Func<Task<T>> taskFactory, CancellationToken cancellationToken = new())
        : base(() => Task.Factory.StartNew(taskFactory, cancellationToken).Unwrap())
    {}

    public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
    
    public override string ToString() => $@"Underlying Type: {typeof(T).Name}{Environment.NewLine}Status: {Value.Status}";
}