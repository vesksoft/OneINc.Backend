using System.ComponentModel.DataAnnotations;

namespace OneINc.Web.Common.Models
{
    public class BasicAuthOptions
    {
        public const string Name = "BasicAuthSettings";
        [Required]
        public string? NameValue { get; set; }
        [Required]
        public string? PasswordValue { get; set; }
    }
}
