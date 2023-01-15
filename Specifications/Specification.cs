namespace TheOmenDen.Shared.Specifications;
public abstract record Specification<T>
{
    #region Properties & Members
    /// <summary>
    /// Returns what the caller defines as an "Identity" - e.g. think of identity properties from mathematics
    /// </summary>
    public static readonly Specification<T> Identity = new IdentitySpecification<T>();

    /// <summary>
    /// Evaluates the results of the chained specifications to ensure that requirements are appropriately satisfied.
    /// </summary>
    /// <param name="entity">The <typeparamref name="T"/> entity that the specification is operating over</param>
    /// <returns><see langword="true" /> when specifications evaluate their conditions correctly, <see langword="false"/> otherwise</returns>
    public Boolean IsSatisfiedBy(T entity)
    {
        var predicate = ToExpression().Compile();
        return predicate(entity);
    } 

    public abstract Expression<Func<T,bool>> ToExpression();
    #endregion
    #region Logic Operations
    /// <summary>
    /// Combines this specification with the supplied <paramref name="specification"/>, and evaluates them with a logical <c>AND</c> operation
    /// </summary>
    /// <param name="specification"></param>
    /// <returns>The combined <see cref="Specification{T}"/>s</returns>
    public Specification<T> And(Specification<T> specification)
    {
        if (this == Identity)
        {
            return specification;
        }

        if (specification == Identity)
        {
            return this;
        }

        return new AndSpecification<T>(this, specification);
    }
    /// <summary>
    /// Combines this specification with another, and evaluates them with a logical <c>NAND (Bubbled Or)</c> operation
    /// </summary>
    /// <param name="specification">The <typeparamref name="T"/> entity that the specification is operating over</param>
    /// <returns>The combined <see cref="Specification{T}"/>s</returns>
    public Specification<T> Nand(Specification<T> specification) 
    {
        if (this == Identity)
        {
            return specification;
        }

        if (specification == Identity)
        {
            return this;
        }
        return new NandSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines this specification with the supplied <paramref name="specification"/>, and evaluates them with a logical <c>OR</c> operation
    /// </summary>
    /// <param name="specification">The <typeparamref name="T"/> entity that the specification is operating over</param>
    /// <returns>The combined <see cref="Specification{T}"/>s</returns>
    public Specification<T> Or(Specification<T> specification)
    {
        if (this == Identity)
        {
            return specification;
        }

        if (specification == Identity)
        {
            return this;
        }

        return new OrSpecification<T>(this, specification);
    }

    /// <summary>
    /// Combines this specification with the supplied <paramref name="specification"/>, and evaluates them with a logical <c>NOR (Bubbled And)</c> operation
    /// </summary>
    /// <param name="specification">The <typeparamref name="T"/> entity that the specification is operating over</param>
    /// <returns>The combined <see cref="Specification{T}"/>s</returns>
    public Specification<T> Nor(Specification<T> specification)
    {
        if (this == Identity)
        {
            return specification;
        }

        if (specification == Identity)
        {
            return this;
        }

        return new NorSpecification<T>(this, specification);
    }
    /// <summary>
    /// Inverts the logical operation over this specification
    /// </summary>
    /// <returns>A <see cref="Specification{T}"/> with a negate value</returns>
    public Specification<T> Not() => new NegatedSpecification<T>(this);
    #endregion
    #region Operators
    public static Specification<T> operator &(Specification<T> lhs, Specification<T> rhs) => lhs.And(rhs);
    public static Specification<T> operator |(Specification<T> lhs, Specification<T> rhs) => lhs.Or(rhs);
    public static Specification<T> operator !(Specification<T> specification) => specification.Not();
    #endregion
}
