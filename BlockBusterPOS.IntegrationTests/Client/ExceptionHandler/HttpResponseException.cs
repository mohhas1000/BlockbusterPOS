namespace BlockBusterPOS.IntegrationTests.Client.Exception;

[Serializable]
public class HttpResponseException : System.Exception
{
    public int StatusCode { get; private set; }

    public string ErrorResponse { get; private set; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    public HttpResponseException(string errorMessage, int statusCode, string errorResponse, IReadOnlyDictionary<string, IEnumerable<string>> headers, System.Exception innerException)
      : base(FormatMessage(errorMessage, statusCode, errorResponse), innerException)
    {
        StatusCode = statusCode;
        ErrorResponse = errorResponse ?? string.Empty;
        Headers = headers ?? new Dictionary<string, IEnumerable<string>>(); // Ensure Headers is never null
    }

    private static string FormatMessage(string message, int statusCode, string response)
    {
        const int maxResponseLength = 256; // Max length of response to include in the message
        var truncatedResponse = response?.Length > maxResponseLength
            ? response.Substring(0, maxResponseLength)
            : response;

        return $"{message}\n\nStatus: {statusCode}\nResponse:\n{(truncatedResponse ?? "(null)")}";
    }

    public override string ToString()
    {
        return "HTTP Response: \n\n" + ErrorResponse + "\n\n" + base.ToString();
    }
}

public class HttpResponseException<TResult> : HttpResponseException
{
    public TResult ExceptionResult { get; private set; }

    public HttpResponseException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, System.Exception innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        ExceptionResult = result;
    }
}