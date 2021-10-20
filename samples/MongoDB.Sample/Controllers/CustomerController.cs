using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Boilerplate.Common.Data;

namespace Boilerplate.MongoDB.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IRepository<Customer, string> _repository;

        public CustomerController(ILogger<CustomerController> logger, IRepository<Customer, string> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return Ok(_repository.Read());
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
            return CreatedAtRoute(nameof(GetCustomer), new { Id = customer.Id }, customer);
        }

        [HttpPatch]
        public ActionResult<Customer> UpdateCustomer(CustomerUpdateDto model)
        {
            var customer = _repository.Update(new Customer {
                Id = model.Id,
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
}
