using Boilerplate.Common.Data;
using Boilerplate.EntityFrameworkCore.Sample.Dtos;
using Boilerplate.EntityFrameworkCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.EntityFrameworkCore.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IAsyncRepository<Customer, long> repository;

    public CustomerController(IAsyncRepository<Customer, long> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return Ok(await repository.ReadAsync());
    }

    [HttpGet("{id:long}", Name = "GetCustomerById")]
    public async Task<ActionResult<Customer>> GetCustomer(long id)
    {
        var customer = await repository.GetByIdAsync(id);

        if (customer is null) {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer(CustomerCreateDto model)
    {
        var customer = await repository.CreateAsync(new Customer {
            FirstName = model.FirstName,
            LastName = model.LastName
        });
        return CreatedAtRoute("GetCustomerById", new { customer.Id }, customer);
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(long id, CustomerUpdateDto model)
    {
        var customer = await repository.UpdateAsync(new Customer {
            Id = id,
            FirstName = model.FirstName,
            LastName = model.LastName
        });
        return Ok(customer);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteCustomer(long id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}
