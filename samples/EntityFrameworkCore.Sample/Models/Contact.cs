using Boilerplate.Common.Data;

namespace Boilerplate.EntityFrameworkCore.Sample.Models;

public class Contact : IEntity<long>
{
    public long Id { get; set; }

    public long OwnerId { get; set; }
    public Customer Owner { get; set; }
}

public class EmailContact : Contact
{
    public string Value { get; set; }
    public bool Suppressed { get; set; }
}

public class PhoneContact : Contact
{
    public string Value { get; set; }
    public bool OptedIn { get; set; }
}
