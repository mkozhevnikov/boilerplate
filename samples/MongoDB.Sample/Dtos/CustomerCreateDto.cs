using System.ComponentModel.DataAnnotations;

namespace Boilerplate.MongoDB.Sample.Dtos;

public class CustomerCreateDto
{
    [Required] public string Name { get; set; }

    public int Age { get; set; }

    public double Balance { get; set; }
}
