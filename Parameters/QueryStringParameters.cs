namespace TheOmenDen.Shared.Parameters;

/// <summary>
/// It provides a base entity for queries from the api, and 
/// <paramref name="IsHistoricalQuery" /> tells us if we are dealing with system version-ed data
/// </summary>
/// <param name="IsHistoricalQuery"><see cref="Boolean"/> to notify if the caller wants to query the underlying history table</param>
public abstract record QueryStringParameters(bool IsHistoricalQuery = false)
{
    protected QueryStringParameters()
        : this(false)
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
}