using System.ComponentModel.DataAnnotations;

namespace OneINc.Web.Common.Models
{
    public class FrontendAppOptions
    {
        public const string Name = "FrontendAppSettings";
        [Required]
        public string HttpUrl { get; set; }
        [Required]
        public string HttpsUrl { get; set; }
    }
}
