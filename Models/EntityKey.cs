namespace TheOmenDen.Shared.Models;
#nullable disable
/// <summary>
/// <inheritdoc cref="IEntityKey"/> 
/// </summary>
public sealed record EntityKey : IEntityKey
{
    private EntityKey(Guid? id, DateTime? createdAt, ITenant tenant, IUser creator)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = createdAt ?? DateTime.UtcNow;
        Tenant = tenant;
        Creator = creator;
    }

    /// <summary>
    /// Creates a new <see cref="EntityKey"/> instance
    /// </summary>
    /// <param name="id">A unique Id</param>
    /// <param name="createdAt">The time created</param>
    /// <param name="tenant">The tenant used</param>
    /// <param name="creator">The originator</param>
    /// <returns>The newly created key</returns>
    public static EntityKey Create(Guid id, DateTime createdAt, Tenant tenant, User creator)
    => new(id, createdAt, tenant, creator);


    public Guid Id { get; }

    public DateTime CreatedAt { get; }

    public ITenant Tenant { get; }

    public IUser Creator { get; }

    public bool Equals(EntityKey other) => 
            other is not null
            && other.Id == Id
            && other.Tenant == Tenant
            && other.Creator == Creator;

    public override int GetHashCode() => HashCode.Combine(Id, CreatedAt, Tenant);
}
