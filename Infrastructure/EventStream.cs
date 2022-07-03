using System.Collections;
using System.Collections.Concurrent;
using TheOmenDen.Shared.Models;

namespace TheOmenDen.Shared.Infrastructure;

public class EventStream: IEnumerable<Event>, IEquatable<EventStream>, IDisposable
{
    private readonly ConcurrentBag<Event> _events = new (new List<Event>(100));

    private readonly Guid _aggregateId;

    public EventStream(Guid aggregateId)
    {
        _aggregateId = aggregateId;
    }

    public EventStream(Guid aggregateId, IEnumerable<Event> events)
    {
        _aggregateId = aggregateId;
        _events = new(events.ToList());
    }

    public Guid Aggregate => _aggregateId;

    public Int32 Count => _events.Count;

    public void ApplyEvent(Event @event)
    {
        _events.Add(@event);
    }

    

    public IReadOnlyList<Event> ReplayEvents()
    {
        return _events.ToList().AsReadOnly();
    }

    public IEnumerator<Event> GetEnumerator()
    {
        return _events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Dispose()
    {
        _events.Clear();
        GC.SuppressFinalize(this);
    }

    public bool Equals(EventStream other)
    {
        return other is not null && _aggregateId == other._aggregateId;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is EventStream other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_aggregateId, _events);
    }
}
