using Boilerplate.Common.Data;

namespace Boilerplate.EntityFrameworkCore.Sample.Models;

public class Customer : IEntity<long>
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public EmailContact PrimaryEmail { get; set; }
    public IEnumerable<EmailContact> Emails { get; set; }

    public PhoneContact PrimaryPhone { get; set; }
    public IEnumerable<PhoneContact> Phones { get; set; }
}
