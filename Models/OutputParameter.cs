namespace TheOmenDen.Shared.Models;

public class OutputParameter<TValue>
{
    private bool _valueSet;

    private TValue _value;

    public TValue Value
    {
        get
        {
            if (!_valueSet)
            {
                throw new InvalidOperationException("Value not set.");
            }

            return _value;
        }

        set => SetValue(value);
    }

    private void SetValue(object? value)
    {
        _valueSet = true;

        _value = value is null || Convert.IsDBNull(value) ? default : (TValue)value;
    }
}
