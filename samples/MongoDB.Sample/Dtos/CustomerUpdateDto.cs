using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample.Dtos;

public class CustomerUpdateDto
{
    [Required]
    public string Name { get; set; }

    public string? Optional { get; set; }
}
