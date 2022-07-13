namespace TheOmenDen.Shared.Models;
#nullable disable
/// <summary>
/// <inheritdoc cref="IEntityKey"/> 
/// </summary>
public record struct EntityKey : IEntityKey
{
    public EntityKey(Guid? id, DateTime? createdAt, Tenant? tenant, User? creator)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = createdAt ?? DateTime.UtcNow;
        Tenant = tenant ?? default;
        Creator = creator ?? default;
    }

    public Guid Id {get;}

    public DateTime CreatedAt { get; }

    public Tenant Tenant { get; }

    public User Creator { get; }

    public bool Equals(EntityKey other)
    {
        return other.Id == Id 
            && other.Tenant == Tenant
            && other.Creator == Creator;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CreatedAt, Tenant);
    }

}
