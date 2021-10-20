using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample
{
    public class CustomerUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}