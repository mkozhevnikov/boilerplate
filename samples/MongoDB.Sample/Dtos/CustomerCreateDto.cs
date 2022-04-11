using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample.Dtos;

public class CustomerCreateDto
{
    [Required]
    public string Name { get; set; }
}
