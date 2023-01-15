namespace TheOmenDen.Shared.Specifications;

/// <summary>
/// Represents a negated operation over an expression (Logical <c>NOT</c>)
/// </summary>
/// <typeparam name="T">The type of entity that the specification is operating over</typeparam>
public record NegatedSpecification<T>(Specification<T> Specification): Specification<T>
{
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = Specification.ToExpression();

        var negatedExpressionBody = Expression.Not(expression.Body);
        
        return Expression.Lambda<Func<T, bool>>(negatedExpressionBody, expression.Parameters.Single());
    }
}
