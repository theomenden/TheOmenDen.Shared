namespace TheOmenDen.Shared.Enumerations.Structs;
/// <summary>
/// A provided consequence of an executed <see cref="Condition{TKey, TEnumeration}"/>
/// </summary>
/// <typeparam name="TKey">The enumeration's key</typeparam>
/// <typeparam name="TEnumeration">The enumeration's value</typeparam>
public readonly record struct Consequence<TKey, TEnumeration>
    where TKey : IEnumerationBase
    where TEnumeration : IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    private readonly Boolean _isMatch;
    
    private readonly IEnumerationBase _enumerationBase;
    
    private readonly Boolean _isEvaluationStopped;

    internal Consequence(Boolean isMatch, Boolean stopEvaluating, IEnumerationBase enumerationBase)
    {
        _isMatch = isMatch;
        _isEvaluationStopped = stopEvaluating;
        _enumerationBase = enumerationBase;
    }

    /// <summary>
    /// Calls the supplied <paramref name="action"/> when the preceding <see cref="Condition{TKey,TEnumeration}"/> is matched
    /// </summary>
    /// <param name="action">Action to call</param>
    /// <returns>a chainable instance of a Conditions for more Consequences</returns>
    public Condition<TKey, TEnumeration> Then(Action action)
    {
        if (!_isEvaluationStopped && _isMatch)
        {
            action();
        }

        return new Condition<TKey, TEnumeration>(_isEvaluationStopped || _isMatch, _enumerationBase);
    }
}