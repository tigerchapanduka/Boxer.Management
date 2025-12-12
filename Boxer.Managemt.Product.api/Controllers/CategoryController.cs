using Boxer.Data;
using Boxer.Management.Service;
using Boxer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static Boxer.Model.Dto;
using Db = Boxer.Management.Product.Data;

namespace Boxer.Managemt.Product.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private Db.DatabaseContext _dbContext;
        private IConfiguration appConfig;
        private ICategoryService _categoryService;
        public CategoryController( Db.DatabaseContext dbContext, IConfiguration config, ICategoryService  categoryService)
        {
            _dbContext = dbContext;
            appConfig = config;
            _categoryService = categoryService;
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostCategory(Model.Category category)
        {
            try
            {
                category.ModifiedById = category.CreatedById;
                Dto.CategoryResponse? response = await _categoryService.Save(category, _dbContext);

                return StatusCode(StatusCodes.Status200OK, response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CategoryResponseList>> GetCategories()
        {
            try
            {
                return await _categoryService.GetCategories(_dbContext);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
