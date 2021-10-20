using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample
{
    public class Customer
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}