namespace TheOmenDen.Shared.Specifications;
/// <summary>
/// Provides functionality to allow for <see cref="Specification{T}"/>s to absorb and use supplied parameters when combined, or negated
/// </summary>
internal sealed class ParameterVisitorReplacer: ExpressionVisitor
{
    private readonly ParameterExpression _parameterExpression;

    protected override Expression VisitParameter(ParameterExpression node) => base.VisitParameter(_parameterExpression);

    internal ParameterVisitorReplacer(ParameterExpression parameterExpression) => _parameterExpression = parameterExpression;
}
