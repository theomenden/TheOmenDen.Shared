namespace TheOmenDen.Shared.Utilities.ConsoleBars;

/// <summary>
/// A basic progress/loading Ellipsis progress utility
/// </summary>
public sealed class ThreadSafeEllipsis : IDisposable, IProgress<Double>
{
    private static readonly object Locker = new();
    
    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1d / 8d);

    private readonly Timer _timer;

    private Double _currentProgress;

    private String _currentText = String.Empty;

    private Boolean _disposed;

    private Int32 _animationIndex;

    public ThreadSafeEllipsis()
    {
        _timer = new Timer(TimerHandler!);

        if (!Console.IsOutputRedirected)
        {
            ResetTimer();
        }
    }

    public void Dispose()
    {
        lock (Locker)
        {
            _disposed = true;
            UpdateText(String.Empty);
            _timer.Dispose();
            StringBuilderPoolFactory<ThreadSafeEllipsis>.Remove(nameof(ThreadSafeEllipsis));
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Reports the progress of a task to the <see cref="Console"/>
    /// </summary>
    /// <param name="value">The current progress value</param>
    public void Report(Double value)
    {
        //Ensures that values is within [0,1]
        value = Math.Max(0, Math.Min(1, value));

        //Ensures that value is updated only once during executions across threads.
        Interlocked.Exchange(ref _currentProgress, value);
    }

    private void TimerHandler(Object state)
    {
        lock (Locker)
        {
            if (_disposed)
            {
                return;
            }

            var text = Animations.Ellipses[_animationIndex++ % Animations.Ellipses.Length].ToString();

            UpdateText(text);

            ResetTimer();
        }
    }

    private void UpdateText(String text)
    {
        var commonPrefixLength = 0;

        var commonLength = Math.Min(_currentText.Length, text.Length);

        while (commonPrefixLength < commonLength &&
               text[commonPrefixLength].Equals(_currentText[commonPrefixLength]))
        {
            commonPrefixLength++;
        }

        var sb = StringBuilderPoolFactory<ThreadSafeEllipsis>.Create(nameof(ThreadSafeEllipsis));

        sb.Append(Animations.NonDestructiveBackspace, _currentText.Length - commonPrefixLength);

        sb.Append(text[commonPrefixLength..]);

        var overlapCount = _currentText.Length - text.Length;

        if (overlapCount > 0)
        {
            sb.Append(Animations.NonDestructiveBackspace, overlapCount);
            sb.Append(Animations.Space, overlapCount);
        }

        Console.Write(sb);
        _currentText = text;
    }

    private void ResetTimer()
    {
        _timer.Change(_animationInterval, TimeSpan.FromMilliseconds(-1));
    }
}
