using Boilerplate.Common.Data;
using Boilerplate.Common.Data.Querying;
using Boilerplate.MongoDB.Sample.Dtos;
using Boilerplate.MongoDB.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.MongoDB.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IRepository<Customer, string> repository;

    public CustomerController(IRepository<Customer, string> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers([FromBody] GetCustomersRequest request)
    {
        return Ok(repository.Read(request.ToSpec<Customer>()));
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomer(string id)
    {
        var customer = repository.GetById(id);

        if (customer is null) {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer(CustomerCreateDto model)
    {
        var customer = repository.Create(new Customer {
            Name = model.Name,
            CreatedOn = DateTime.Today,
            Age = model.Age,
            Balance = model.Balance
        });
        return CreatedAtRoute("GetCustomerById", new { customer.Id }, customer);
    }

    [HttpPatch("{id}")]
    public ActionResult<Customer> UpdateCustomer(string id, CustomerUpdateDto model)
    {
        var customer = repository.Update(new Customer {
            Id = id,
            Name = model.Name,
            Optional = model.Optional
        });
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(string id)
    {
        repository.Delete(id);
        return NoContent();
    }
}

public class GetCustomersRequest : IFilteredListRequest
{
    public FilterDescriptor Filter { get; set; }
}
