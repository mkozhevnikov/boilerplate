using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample
{
    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}