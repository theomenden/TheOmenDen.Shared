namespace TheOmenDen.Shared.Specifications;

/// <summary>
/// Represents a combination of evaluated expressions using a Logical <c>AND</c> operation 
/// </summary>
///<typeparam name="T">The type of entity that the specification is operating over</typeparam>
public record AndSpecification<T>(Specification<T> LeftHandSide, Specification<T> RightHandSide) : Specification<T>
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var lhs = LeftHandSide.ToExpression();
        var rhs = RightHandSide.ToExpression();

        var invokedExpression = Expression.Invoke(rhs, lhs.Parameters);

        return (Expression<Func<T,bool>>)(Expression.Lambda(Expression.AndAlso(lhs.Body, invokedExpression), lhs.Parameters));
    }
}
