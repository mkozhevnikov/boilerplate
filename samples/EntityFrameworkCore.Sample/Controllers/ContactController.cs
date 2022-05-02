using Boilerplate.Common.Data;
using Boilerplate.EntityFrameworkCore.Sample.Dtos;
using Boilerplate.EntityFrameworkCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.EntityFrameworkCore.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IAsyncRepository<Contact, long> repository;

    public ContactController(IAsyncRepository<Contact, long> repository)
    {
        this.repository = repository;
    }

    [HttpGet("{id:long}/")]
    public async Task<ActionResult<Contact>> GetCustomerContacts(long id)
    {
        return Ok(await repository.ReadAsync(new Spec<Contact>(c => c.OwnerId == id)));
    }

    [HttpGet("{id:long}", Name = "GetContactById")]
    public async Task<ActionResult<Contact>> GetContact(long id)
    {
        var contact = await repository.GetByIdAsync(id);

        if (contact is null) {
            return NotFound();
        }

        return Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult<EmailContact>> CreateEmailContact(EmailContactCreateDto model)
    {
        var contact = await repository.CreateAsync(new EmailContact {
            OwnerId = model.OwnerId,
            Value = model.Value
        });
        return CreatedAtRoute("GetContactById", new { contact.Id }, contact);
    }

    [HttpPost]
    public async Task<ActionResult<PhoneContact>> CreatePhoneContact(PhoneContactCreateDto model)
    {
        var contact = await repository.CreateAsync(new PhoneContact {
            OwnerId = model.OwnerId,
            Value = model.Value
        });
        return CreatedAtRoute("GetContactById", new { contact.Id }, contact);
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<EmailContact>> UpdateEmailContact(long id, EmailContactUpdateDto model)
    {
        var contact = await repository.UpdateAsync(new EmailContact {
            Id = id,
            Value = model.Value,
            Suppressed = model.Suppressed
        });
        return Ok(contact);
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<PhoneContact>> UpdatePhoneContact(long id, PhoneContactUpdateDto model)
    {
        var contact = await repository.UpdateAsync(new PhoneContact {
            Id = id,
            Value = model.Value,
            OptedIn = model.OptedIn
        });
        return Ok(contact);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteContact(long id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}
