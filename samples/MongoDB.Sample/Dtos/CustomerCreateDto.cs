namespace Boilerplate.MongoDB.Sample.Dtos;

using System.ComponentModel.DataAnnotations;

public class CustomerCreateDto
{
    [Required]
    public string Name { get; set; }
}
