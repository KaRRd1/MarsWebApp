namespace ApplicationCore.Interfaces;

public interface IUriComposer
{
    string ComposeBaseUri(string url);
    string ComposePictureUri(string uriTemplate);
}