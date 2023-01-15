namespace TheOmenDen.Shared.Specifications;
/// <summary>
/// Represents a combination of evaluated expressions using a Logical <c>NOR (Bubbled And)</c> operation 
/// </summary>
///<typeparam name="T">The type of entity that the specification is operating over</typeparam>
/// <param name="LeftHandSide"></param>
/// <param name="RightHandSide"></param>
public record NorSpecification<T>(Specification<T> LeftHandSide, Specification<T> RightHandSide): Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        //~lhs v ~rhs == ~(lhs ^ rhs)  (De Morgan's)
        var lhs = LeftHandSide.Not().ToExpression();
        var rhs = RightHandSide.Not().ToExpression();

        var invokedExpression = Expression.Invoke(rhs, lhs.Parameters);
        
        return (Expression<Func<T,bool>>)Expression.Lambda(Expression.AndAlso(lhs.Body, invokedExpression), lhs.Parameters);
    }
}
