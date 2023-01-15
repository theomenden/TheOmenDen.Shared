namespace TheOmenDen.Shared.Specifications;

public sealed record IdentitySpecification<T>: Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => entity => true;
}
