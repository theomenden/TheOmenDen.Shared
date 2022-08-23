namespace TheOmenDen.Shared.Enumerations.Structs;

/// <summary>
/// A given condition with a reasonable action to execute to evaluate to <see cref="Consequence{TKey, TEnumeration}"/>
/// </summary>
/// <typeparam name="TKey">The enumeration's base key</typeparam>
/// <typeparam name="TEnumeration">The enumeration's value</typeparam>
public readonly record struct Condition<TKey, TEnumeration>
    where TKey : IEnumerationBase
    where TEnumeration : IEquatable<TEnumeration>, IComparable<TEnumeration>
{
    private readonly IEnumerationBase _enumerationBase;

    private readonly bool _isEvaluationStopped;

    internal Condition(Boolean stopEvaluation, IEnumerationBase enumerationBase)
    {
        _enumerationBase = enumerationBase;
        _isEvaluationStopped = stopEvaluation;
    }

    /// <summary>
    /// Executable action when no other calls to this consequence have been matched
    /// </summary>
    /// <param name="action">The action to execute</param>
    public void DefaultCondition(Action action)
    {
        if (!_isEvaluationStopped)
        {
            action();
        }
    }
    #region Execution Methods
    /// <summary>
    /// When the given condition is matched by the specified <see cref="EnumerationBase{TKey,TEnumeration}"/> parameters, then we retrieve the <see cref="Consequence{TKey, TEnumeration}"/>
    /// </summary>
    /// <param name="enumerationBaseConditional">A supplied <see cref="EnumerationBase{TKey,TEnumeration}"/> values to compare to in this instance</param>
    /// <returns>An execution path for the supplied action</returns>
    public Consequence<TKey, TEnumeration> When(IEnumerationBase enumerationBaseConditional) =>
        new (
            isMatch: _enumerationBase.Equals(enumerationBaseConditional),
            stopEvaluating: _isEvaluationStopped,
            enumerationBase: _enumerationBase
        );
    /// <summary>
    /// When the given condition is matched by the specified <see cref="EnumerationBase{TKey,TEnumeration}"/> parameters, then we retrieve the <see cref="Consequence{TKey, TEnumeration}"/>
    /// </summary>
    /// <param name="enumerationBaseConditionals">A collection of <see cref="EnumerationBase{TKey,TEnumeration}"/> values to compare to in this instance</param>
    /// <returns>An execution path for the supplied action</returns>
    public Consequence<TKey, TEnumeration> When(IEnumerable<IEnumerationBase> enumerationBaseConditionals) =>
        new (isMatch: enumerationBaseConditionals.Contains(_enumerationBase),
            stopEvaluating: _isEvaluationStopped,
            enumerationBase: _enumerationBase);
    /// <summary>
    /// When the given condition is matched by the specified <see cref="EnumerationBase{TKey,TEnumeration}"/> parameters, then we retrieve the <see cref="Consequence{TKey, TEnumeration}"/>
    /// </summary>
    /// <param name="enumerationBaseConditionals">A collection of <see cref="EnumerationBase{TKey,TEnumeration}"/> values to compare to in this instance</param>
    /// <returns>An execution path for the supplied action</returns>
    public Consequence<TKey, TEnumeration> When(params IEnumerationBase[] enumerationBaseConditionals) =>
        new(isMatch: enumerationBaseConditionals.Contains(_enumerationBase),
            stopEvaluating: _isEvaluationStopped,
            enumerationBase: _enumerationBase);
    #endregion
}