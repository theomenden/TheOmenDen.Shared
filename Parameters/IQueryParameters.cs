namespace TheOmenDen.Shared.Parameters;
public interface IQueryParameters
    {
        Boolean IsHistoricalQuery { get; }

        Guid Id { get; }
    }