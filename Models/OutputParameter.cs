namespace TheOmenDen.Shared.Models;

public class OutputParameter<TValue>
{
    private bool _valueSet = false;

    public TValue _value;
    public TValue Value
    {
        get
        {
            if (!_valueSet)
                throw new InvalidOperationException("Value not set.");

            return _value;
        }
    }

    internal void SetValue(object value)
    {
        _valueSet = true;

        _value = value is null || Convert.IsDBNull(value) ? default(TValue) : (TValue)value;
    }
}
