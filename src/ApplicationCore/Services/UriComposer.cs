using ApplicationCore.Interfaces;

namespace ApplicationCore.Services;

public class UriComposer : IUriComposer
{
    private const string BaseUrlToBeReplaced = "https://baseurl";
    private readonly MarsSettings _marsSettings;

    public UriComposer(MarsSettings marsSettings) => _marsSettings = marsSettings;

    public string ComposeBaseUri(string url)
    {
        return BaseUrlToBeReplaced + url;
    }

    public string ComposePictureUri(string uriTemplate)
    {
        return uriTemplate.Replace(BaseUrlToBeReplaced, _marsSettings.MarsBaseUrl);
    }
}