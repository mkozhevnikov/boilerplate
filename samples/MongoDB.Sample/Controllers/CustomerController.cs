using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Boilerplate.MongoDB.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return null;
        }

        [HttpGet("{id}", Name = "GetCustomerById")]
        public ActionResult<Customer> GetCustomer(string id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult<Customer> CreateCustomer(CustomerCreateDto model)
        {
            return null;
        }

        [HttpPatch]
        public ActionResult<Customer> UpdateCustomer(CustomerUpdateDto model)
        {
            return null;
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCustomer(string id)
        {
            return null;
        }
    }
}
