namespace TheOmenDen.Shared.Utilities.ConsoleBars;

/// <summary>
/// An implementation of a thread safe progress bar
/// <inheritdoc cref="IDisposable"/>
/// <inheritdoc cref="IProgress{T}"/>
/// </summary>
public sealed class ThreadSafeBar : IDisposable, IProgress<Double>
{
    private const Int32 BlockCount = 10;

    private readonly TimeSpan _animationInterval = TimeSpan.FromSeconds(1.0 / 8);

    private Boolean _showProgressbar = true;

    private readonly Timer _timer;

    private Double _currentProgress;

    private String _currentText = String.Empty;

    private Boolean _disposed;

    private Int32 _animationIndex;

    public ThreadSafeBar(Boolean showProgressbar = true)
    {
        _showProgressbar = showProgressbar;

        _timer = new Timer(TimerHandler!);

        if (!Console.IsOutputRedirected)
        {
            ResetTimer();
        }
    }

    public void Dispose()
    {
        lock (_timer)
        {
            _disposed = true;
            UpdateText(String.Empty);
            _timer.Dispose();
            StringBuilderPoolFactory<ThreadSafeBar>.Remove(nameof(ThreadSafeBar));
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Reports the current stage of a task
    /// </summary>
    /// <param name="value">The current progress</param>
    public void Report(Double value)
    {
        //Ensures that values is within [0,1]
        value = Math.Max(0, Math.Min(1, value));

        //Ensures that value is updated only once during executions across threads.
        Interlocked.Exchange(ref _currentProgress, value);
    }

    private void TimerHandler(Object state)
    {
        lock (_timer)
        {
            if (_disposed)
            {
                return;
            }

            String text;

            if (_showProgressbar)
            {
                var progressBlockCount = (int)(_currentProgress * BlockCount);
                var percent = (int)(_currentProgress * 100);

                text =
                    $"[{new String('#', progressBlockCount)}{new String('-', BlockCount - progressBlockCount)}] {percent}% {Animations.Twirl[_animationIndex++ % Animations.Twirl.Length]}";
            }
            else
            {
                text = Animations.Twirl[_animationIndex++ % Animations.Twirl.Length].ToString();
            }

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

        var sb = StringBuilderPoolFactory<ThreadSafeBar>.Create(nameof(ThreadSafeBar));

        sb.Append(Animations.NonDestructiveBackspace, _currentText.Length - commonPrefixLength);

        sb.Append(text[commonPrefixLength..]);

        var overlapCount = _currentText.Length - text.Length;

        if (overlapCount > 0)
        {
            sb.Append(Animations.Space, overlapCount);
            sb.Append(Animations.NonDestructiveBackspace, overlapCount);
        }

        Console.Write(sb);
        _currentText = text;
    }

    private void ResetTimer()
    {
        _timer.Change(_animationInterval, TimeSpan.FromMilliseconds(-1));
    }
}
