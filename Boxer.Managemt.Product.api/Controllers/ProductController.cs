using Azure;
using Azure.Core;
using Boxer.Data;
using Boxer.Management.Service;
using Boxer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static Boxer.Model.Dto;
using Db =Boxer.Management.Product.Data;

namespace Boxer.Managemt.Product.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private Db.DatabaseContext _context;
        private IConfiguration appConfig;
        IProductService producstService;

        public ProductController(Db.DatabaseContext context, IConfiguration config, IProductService productservice)
        {
            _context = context;
            appConfig = config;
            producstService = productservice;
        }
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> PostProduct(Model.Dto.ProductRequest product)
        {
            try
            {
                String validationResult = validate(product).ToString();
                if (!string.IsNullOrEmpty(validationResult))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, validationResult);
                }

                Dto.ProductResponse? response = await producstService.Save(product, _context);
                
                if (!string.IsNullOrEmpty(response.errorMessage))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, response.errorMessage);
                }

                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,new ProductResponse(ex.Message,"",null,null,null,null,null));
            }
        }
        [HttpGet]
        public async Task<ActionResult<ProductResponseList>> GetProducts()
        {
            try
            {
                return await producstService.GetProducts(_context);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(Guid id)
        {
            var response = await producstService.GetProduct(id, _context);

            if (response == null)
            {
                return NotFound();

            }else if (!string.IsNullOrEmpty(response.errorMessage))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.errorMessage);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }
        [HttpGet("{categoryname}/{productname}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(string categoryname, string productname)
        {
            var response = await producstService.GetFilteredProducts(categoryname,productname, _context);

            if (response == null)
            {
                return NotFound();

            }
            else if (!string.IsNullOrEmpty(response.errorMessage))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.errorMessage);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
              ProductResponse response = await producstService.DeleteProduct(id,_context);
                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProductResponse(ex.Message, null, null, null, null,null, null)); 
            }

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductRequest request)
        {
            String validationResult = validate(request).ToString();
            if (!string.IsNullOrEmpty(validationResult)) {
                return StatusCode(StatusCodes.Status500InternalServerError, validationResult);
            }

            if (id != new Guid(request.id))
            {
                return BadRequest();
            }

            ProductResponse response = await producstService.UpdateProduct(request, _context);
            if (!string.IsNullOrEmpty(response.errorMessage)) {
              return  StatusCode(StatusCodes.Status500InternalServerError,response.errorMessage);
            }
            return StatusCode(StatusCodes.Status200OK,response);
        }
        private StringBuilder validate(ProductRequest request) {
            StringBuilder errorBuilder = new StringBuilder();
            if (String.IsNullOrEmpty(request.name)) {
                errorBuilder.AppendLine("name is required");
            }
            if (String.IsNullOrEmpty(request.categoryId))
            {
                errorBuilder.AppendLine("Category is required");
            }
            if (String.IsNullOrEmpty(request.code))
            {
                errorBuilder.AppendLine("Code is required");
            }
            if (!request.cost.HasValue || request.cost==0)
            {
                errorBuilder.AppendLine("Cost is required");
            }
            return errorBuilder;
        }

    }
}
