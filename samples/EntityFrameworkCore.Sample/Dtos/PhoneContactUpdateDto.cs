using System.ComponentModel.DataAnnotations;

namespace Boilerplate.EntityFrameworkCore.Sample.Dtos;

public class PhoneContactUpdateDto
{
    [Required] public string Value { get; set; }
    public bool OptedIn { get; set; }
}
