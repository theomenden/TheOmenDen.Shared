namespace TheOmenDen.Shared.Models;

/// <summary>
/// Provides basic information about a tenant
/// </summary>
/// <param name="Id">Tenant's unique Id</param>
/// <param name="Code">Provided Tenant Code</param>
/// <param name="Name">A tenant's Name</param>
/// <param name="Key">A particular string based key</param>
public sealed record Tenant(Guid Id, String Code, String Name, Int32 Key);