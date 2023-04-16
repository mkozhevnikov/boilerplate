using System.ComponentModel.DataAnnotations;
using Boilerplate.Common.Data;

namespace Boilerplate.MongoDB.Sample.Models;

public class Customer : IEntity<string>
{
    [Key] [Required] public string Id { get; set; }

    [Required] public string Name { get; set; }

    public int Age { get; set; }

    public DateTime CreatedOn { get; set; }

    public double Balance { get; set; }

    public string? Optional { get; set; }
}
