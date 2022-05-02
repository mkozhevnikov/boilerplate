using System.ComponentModel.DataAnnotations;

namespace Boilerplate.EntityFrameworkCore.Sample.Dtos;

public class EmailContactUpdateDto
{
    [Required] public string Value { get; set; }
    public bool Suppressed { get; set; }
}
