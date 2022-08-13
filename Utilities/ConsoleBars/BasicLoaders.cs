namespace TheOmenDen.Shared.Utilities.ConsoleBars;

/// <summary>
/// A basic progress indicator console utility
/// </summary>
public sealed class BasicLoaders
{
    #region Loading Bar
    /// <summary>
    /// Creates a basic loading bar
    /// </summary>
    /// <param name="percent">The current whole number percentage</param>
    /// <remarks>Changes the console foreground (Text) color to: <see cref="ConsoleColor.Magenta"/></remarks>
    public static void LoadingBarIndicator(Int32 percent)
    {
        LoadingBarIndicator(percent, isUpdating: true);
    }

    /// <summary>
    /// Creates a basic loading bar
    /// </summary>
    /// <param name="percent">The current whole number percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    public static void LoadingBarIndicator(Int32 percent, ConsoleColor loadingColor)
    {
        LoadingBarIndicator(percent, loadingColor, true);
    }

    /// <summary>
    /// Creates a basic loading bar
    /// </summary>
    /// <param name="percent">The current whole number percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    /// <param name="isUpdating">Flag indicating if we're updating an existing bar</param>
    public static void LoadingBarIndicator(Int32 percent, ConsoleColor loadingColor = ConsoleColor.Magenta, Boolean isUpdating = false)
    {
        Console.ForegroundColor = loadingColor;

        if (isUpdating)
        {
            Console.Write(new string(Animations.NonDestructiveBackspace, 18));
        }

        Console.Write(Animations.LeftBracket);

        var p = (int)(percent / 10f + 0.5f);

        for (var i = 0; i < 10; ++i)
        {
            if (i >= p)
            {
                Console.Write(Animations.Space);
                continue;
            }

            Console.Write(Animations.Hashtag);
        }

        Console.Write($"{Animations.RightBracket}{Animations.Space}" + @"{0,3:##0}%", percent);

        if (percent >= 100)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Animations.Space}{Animations.Completed}");
        }

        Console.ResetColor();
    }
    #endregion
    #region Twirling Indicator
    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <remarks>Changes the console foreground (Text) color to: <see cref="ConsoleColor.Magenta"/></remarks>
    public static void TwirlingProgressIndicator(Int32 progress)
    {
        TwirlingProgressIndicator(progress, isUpdating: true);
    }

    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    public static void TwirlingProgressIndicator(Int32 progress, ConsoleColor loadingColor)
    {
        TwirlingProgressIndicator(progress, loadingColor, true);
    }

    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    /// <param name="isUpdating">Indicates if we're updating or creating the bar</param>
    public static void TwirlingProgressIndicator(Int32 progress, ConsoleColor loadingColor = ConsoleColor.Magenta, Boolean isUpdating = false)
    {
        Console.ForegroundColor = loadingColor;

        if (isUpdating)
        {
            Console.Write(Animations.NonDestructiveBackspace);
        }

        Console.Write(Animations.Twirl[progress % Animations.Twirl.Length]);

        if (progress >= 100)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Animations.Space}{Animations.Completed}");
        }

        Console.ResetColor();
    }
    #endregion
    #region Ellipsis Indicator
    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <remarks>Changes the console foreground (Text) color to: <see cref="ConsoleColor.Magenta"/></remarks>
    public static void EllipsisProgressIndicator(Int32 progress)
    {
        EllipsisProgressIndicator(progress, isUpdating: true);
    }

    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    public static void EllipsisProgressIndicator(Int32 progress, ConsoleColor loadingColor)
    {
        EllipsisProgressIndicator(progress, loadingColor, true);
    }

    /// <summary>
    /// Creates a basic twirling progress indicator on the console
    /// </summary>
    /// <param name="progress">The whole number progress percentage</param>
    /// <param name="loadingColor">The color for the console text</param>
    /// <param name="isUpdating">Indicates if we're updating or creating the bar</param>
    public static void EllipsisProgressIndicator(Int32 progress, ConsoleColor loadingColor = ConsoleColor.Magenta, Boolean isUpdating = false)
    {
        Console.ForegroundColor = loadingColor;

        if (isUpdating)
        {
            Console.Write(Animations.NonDestructiveBackspace);
        }

        Console.Write(new String(Animations.Space, 4) + new String(Animations.NonDestructiveBackspace, Animations.Ellipses.Length + 3)+ Animations.Ellipses);

        if (progress >= 100)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"{Animations.Space}{Animations.Completed}");
        }

        Console.ResetColor();
    }
    #endregion
}
