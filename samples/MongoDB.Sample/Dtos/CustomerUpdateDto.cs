namespace Boilerplate.MongoDB.Sample.Dtos;

using System.ComponentModel.DataAnnotations;

public class CustomerUpdateDto
{
    [Required]
    public string Name { get; set; }
}