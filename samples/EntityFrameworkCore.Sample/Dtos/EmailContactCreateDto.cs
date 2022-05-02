using System.ComponentModel.DataAnnotations;

namespace Boilerplate.EntityFrameworkCore.Sample.Dtos;

public class EmailContactCreateDto
{
    [Required] public long OwnerId { get; set; }
    [Required] public string Value { get; set; }
}
