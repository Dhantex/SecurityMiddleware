namespace SecurityMiddleware.api.Interface
{
    public interface ISecureDomainPolicy
    {
        string ScriptSrc { get; set; }
        string StyleSrc { get; set; }
        string ImgSrc { get; set; }
        string FontSrc { get; set; }
        string ConnectSrc { get; set; }
        string FormAction { get; set; }
        string FrameSrc { get; set; }
    }
}
