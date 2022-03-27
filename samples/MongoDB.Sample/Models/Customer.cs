namespace Boilerplate.MongoDB.Sample.Models;

using System.ComponentModel.DataAnnotations;
using Common.Data;

public class Customer : IEntity<string>
{
    [Key]
    [Required]
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
}
