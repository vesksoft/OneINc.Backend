using System.ComponentModel.DataAnnotations;

namespace OneINc.Web.Common.Models
{
    public class RandomPauseOptions
    {
        public const string Name = "RandomPauseSettings";
        [Required]
        public int StartValue { get; set; }
        [Required]
        public int EndValue { get; set; }
    }
}
