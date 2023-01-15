namespace TheOmenDen.Shared.Specifications;
/// <summary>
/// Represents a combination of evaluated expressions using a Logical <c>OR</c> operation 
/// </summary>
///<typeparam name="T">The type of entity that the specification is operating over</typeparam>
/// <param name="LeftHandSide"></param>
/// <param name="RightHandSide"></param>
public record OrSpecification<T>(Specification<T> LeftHandSide, Specification<T> RightHandSide) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var lhs = LeftHandSide.ToExpression();
        var rhs = RightHandSide.ToExpression();

        var invokedExpression = Expression.Invoke(rhs, lhs.Parameters);

        return (Expression<Func<T, bool>>)(Expression.Lambda(Expression.OrElse(lhs.Body, invokedExpression), lhs.Parameters));
    }
}
