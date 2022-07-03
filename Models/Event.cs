#nullable disable
namespace TheOmenDen.Shared.Models;

public abstract class Event: IEntity
{
    protected Event()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public int MajorVersion { get; set; }
    
    public int MinorVersion { get; set; }
   
    public string Name { get; set; }
    
    public Type AggregateType { get; set; }

    public string Data { get; set; }
    
    public Guid AggregateId { get; set; }
    
    public string Aggregate { get; set; }
}