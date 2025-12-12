using Boxer.Management.Product.Data;
using Boxer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Service
{
    public interface ICategoryService
    {
        public Task<Dto.CategoryResponse> Save(Model.Category category, DatabaseContext dbcontext);
        public Task<Dto.CategoryResponseList> GetCategories(DatabaseContext dbcontext);

    }
}
