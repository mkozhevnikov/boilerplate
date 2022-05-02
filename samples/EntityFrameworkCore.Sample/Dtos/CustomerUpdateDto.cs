using System.ComponentModel.DataAnnotations;

namespace Boilerplate.EntityFrameworkCore.Sample.Dtos;

public class CustomerUpdateDto
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
}
