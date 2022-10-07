namespace TheOmenDen.Shared.Models;

/// <summary>
/// Provides basic information about a tenant
/// </summary>
/// <param name="Id">Tenant's unique Id</param>
/// <param name="Code">Provided Tenant Code</param>
/// <param name="Name">A tenant's Name</param>
/// <param name="Key">A particular string based key</param>
public sealed record Tenant(Guid Id, String Code, String Name, Int32 Key) : ITenant
{
    public bool Equals(Tenant? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other.Id == Id
               && Code == other.Code 
               && Key == other.Key;
    }

    public override int GetHashCode() => HashCode.Combine(Id, Code, Key);
}