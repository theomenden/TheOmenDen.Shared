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
    #region Private Members
    private readonly IEnumerationBase _enumerationBase;

    private readonly bool _isEvaluationStopped;
    #endregion
    #region Constructors
    internal Condition(Boolean stopEvaluation, IEnumerationBase enumerationBase)
    {
        _enumerationBase = enumerationBase;
        _isEvaluationStopped = stopEvaluation;
    }
    #endregion
    #region Default Actions and Conditions
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

    /// <summary>
    /// Executable <paramref name="action" /> that can be run using <see langword="async"/>...<seealso langword="await"/> lambda expressions
    /// </summary>
    /// <param name="action">The action we want to run</param>
    /// <remarks><paramref name="action"/> is called with <c>ConfigureAwait(false)</c></remarks>
    public async Task DefaultAsyncCondition(Func<Task> action)
    {
        if (!_isEvaluationStopped)
        {
            await action().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Executable <paramref name="action" /> that can be run using <see langword="async"/>...<seealso langword="await"/> lambda expressions, with <see cref="CancellationToken"/> support
    /// </summary>
    /// <param name="action">The action we want to run</param>
    /// <param name="cancellationToken"></param>
    /// <remarks><paramref name="action"/> is called with <c>ConfigureAwait(false)</c></remarks>
    public async Task DefaultAsyncConditionWithCancellation(Func<Task> action, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.FromCanceled(cancellationToken).ConfigureAwait(false);
        }

        if (_isEvaluationStopped)
        {
            await action().ConfigureAwait(false);
        }
    }
    #endregion
    #region Execution Methods
    /// <summary>
    /// When the given condition is matched by the specified <see cref="EnumerationBase{TKey,TEnumeration}"/> parameters, then we retrieve the <see cref="Consequence{TKey, TEnumeration}"/>
    /// </summary>
    /// <param name="enumerationBaseConditional">A supplied <see cref="EnumerationBase{TKey,TEnumeration}"/> values to compare to in this instance</param>
    /// <returns>An execution path for the supplied action</returns>
    public Consequence<TKey, TEnumeration> When(IEnumerationBase enumerationBaseConditional) =>
        new(
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
        new(isMatch: enumerationBaseConditionals.Contains(_enumerationBase),
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