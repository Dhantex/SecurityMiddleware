using SecurityMiddleware.api.Interface;

namespace SecurityMiddleware.api.Models
{
    public class SecureDomainPolicy : ISecureDomainPolicy
    {
        public string ScriptSrc { get; set; }
        public string StyleSrc { get; set; }
        public string ImgSrc { get; set; }
        public string FontSrc { get; set; }
        public string ConnectSrc { get; set; }
        public string FormAction { get; set; }
        public string FrameSrc { get; set; }
    }
}
