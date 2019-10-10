using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Product.Models;
using Product.Repositories;
using System.Net;
using System.Threading.Tasks;

namespace Product.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository context;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public ProductController(ProductRepository context, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        // POST: api/products
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.CreateAsync(product));
        }

        // GET: products/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.GetAsync(id));
        }

        // POST: products/search
        [HttpPost("search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchProducts([FromBody] ProductModel product)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAsync(product));
        }

        // DELETE: products/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.DeleteAsync(id));
        }

        // PUT: products/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateAsync(id, product));
        }
    }
}