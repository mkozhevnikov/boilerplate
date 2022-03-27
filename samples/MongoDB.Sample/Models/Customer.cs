using System.ComponentModel.DataAnnotations;
using Boilerplate.Common.Data;

namespace Boilerplate.MongoDB.Sample.Models
{
    public class Customer : IEntity<string>
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}