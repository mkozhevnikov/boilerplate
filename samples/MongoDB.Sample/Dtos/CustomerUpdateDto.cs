using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample
{
    public class CustomerUpdateDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}