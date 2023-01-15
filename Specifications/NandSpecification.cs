namespace TheOmenDen.Shared.Specifications;
/// <summary>
/// Represents a combination of evaluated expressions using a Logical <c>NAND (Bubbled OR)</c> operation 
/// </summary>
///<typeparam name="T">The type of entity that the specification is operating over</typeparam>
/// <param name="LeftHandSide"></param>
/// <param name="RightHandSide"></param>
public record NandSpecification<T>(Specification<T> LeftHandSide, Specification<T> RightHandSide) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        //~(lhs ^ rhs) == ~lhs v ~rhs (De Morgan's)
        var lhs = LeftHandSide.Not().ToExpression();
        var rhs = RightHandSide.Not().ToExpression();

        var invokedExpression = Expression.Invoke(rhs, lhs.Parameters);

        return (Expression<Func<T,bool>>)Expression.Lambda(Expression.OrElse(lhs.Body, invokedExpression), lhs.Parameters);
    }
}
