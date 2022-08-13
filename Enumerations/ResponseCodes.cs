namespace TheOmenDen.Shared.Enumerations;

/// <summary>
/// A list of codes that are used to help us determine how our Client to API Pipeline is behaving
/// </summary>
public sealed record ResponseCodes: EnumerationBase
{
    private ResponseCodes(String name, Int32 id)
        :base(name, id)
    {
    }

    /// <summary>
    /// HttpError : An Error that occurs during the transmission of the request
    /// </summary>
    public static readonly ResponseCodes HttpError = new(nameof(HttpError), 1);

    /// <summary>
    /// ApiError : An Error that occurred from within processing the request in the api
    /// </summary>
    public static readonly ResponseCodes ApiError = new(nameof(ApiError), 2);

    /// <summary>
    /// UnrecognizedError : The api returned a error that we didn't recognize and were unable to process
    /// </summary>
    public static readonly ResponseCodes UnrecognizedError = new(nameof(UnrecognizedError), 3);

    /// <summary>
    /// UnintelligibleResponse: A non-error response from the api that we didn't recognize and were unable to process
    /// </summary>
    public static readonly ResponseCodes UnintelligibleResponse = new(nameof(UnintelligibleResponse), 4);

    /// <summary>
    /// ApiSuccess : A successful response
    /// </summary>
    public static readonly ResponseCodes ApiSuccess = new(nameof(ApiSuccess), 5);
}