namespace TheOmenDen.Shared.Guards;

public partial class Guard : ICanGuard
{
    private static ICanGuard _against;

    private Guard()
    {
    }

    public static ICanGuard Against
    {
        get
        {
            return _against ?? new Guard();
        }
    }
}