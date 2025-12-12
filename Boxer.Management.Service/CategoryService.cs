using Boxer.Management.Product.Data;
using Boxer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Service
{
    public class CategoryService : ICategoryService
    {
        private IConfiguration appConfig;
          
        public CategoryService(IConfiguration config )
        {
            appConfig = config;
      

        }
        public async Task<Dto.CategoryResponse> Save(Model.Category category, DatabaseContext dbcontext)
        {
            try
            {
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                dbcontext.category.Add(category);
                await dbcontext.SaveChangesAsync();

                return new Dto.CategoryResponse("", category.Name,category.Id.ToString());
            }
            catch (Exception ex)
            {
                return new Dto.CategoryResponse(ex.Message, null,null);

            }
        }
        public async Task<Dto.CategoryResponseList> GetCategories( DatabaseContext dbcontext)
        {
            try
            {

              List<Category> categories= await dbcontext.category.ToListAsync();
                List<Dto.CategoryResponse> categoryResponses = new List<Dto.CategoryResponse>();
                categories.ForEach(category => { 
                categoryResponses.Add( new Dto.CategoryResponse("", category.Name, category.Id.ToString()) );
                } );

                return new Dto.CategoryResponseList("", categoryResponses);
            }
            catch (Exception ex)
            {
                return new Dto.CategoryResponseList(ex.Message, null);

            }
        }
    }
}
