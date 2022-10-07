namespace TheOmenDen.Shared.Models;

/// <summary>
/// Provides basic information about a tenant
/// </summary>
public interface ITenant
{
    /// <summary>
    /// Tenant's unique Id
    /// </summary>
    Guid Id { get; }
    
    /// <summary>
    /// The string Code representing the tenant
    /// </summary>
    String Code { get; }
    
    /// <summary>
    /// A tenant's Name
    /// </summary>
    String Name { get; }

    /// <summary>
    /// A particular integer based key
    /// </summary>
    Int32 Key { get; }
}


