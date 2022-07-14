namespace Web.Extensions;

public static class HttpRequestExtensions
{
    public static string GetPathWithQuery(this HttpRequest httpRequest, bool parameterSeparatorInEnd = false)
    {
        var pathWithQuery = httpRequest.Path + httpRequest.QueryString;

        if (!parameterSeparatorInEnd)
            return pathWithQuery;

        return $@"{pathWithQuery}{(httpRequest.Query.Count == 0 ? '?' : '&')}";
    }
}