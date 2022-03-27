using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Boilerplate.Common.Data;
using Boilerplate.MongoDB.Sample.Dtos;
using Boilerplate.MongoDB.Sample.Models;

namespace Boilerplate.MongoDB.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IRepository<Customer, string> _repository;

    public CustomerController(IRepository<Customer, string> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        return Ok(_repository.Read(Specs.True<Customer>()));
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomer(string id)
    {
        var customer = _repository.GetById(id);
            
        if (customer is null)
            return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public ActionResult<Customer> CreateCustomer(CustomerCreateDto model)
    {
        var customer = _repository.Create(new Customer {
            Name = model.Name
        });
        return CreatedAtRoute("GetCustomerById", new { Id = customer.Id }, customer);
    }

    [HttpPatch("{id}")]
    public ActionResult<Customer> UpdateCustomer(string id, CustomerUpdateDto model)
    {
        var customer = _repository.Update(new Customer {
            Id = id,
            Name = model.Name
        });
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCustomer(string id)
    {
        _repository.Delete(id);
        return NoContent();
    }
}