using Boxer.Management.Product.Data;
using Boxer.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Management.Service
{
    public interface IProductService
    {
       // public void SetDbContext(DatabaseContext dataBaseContext);
        public Task<Dto.ProductResponse> Save(Model.Dto.ProductRequest product, DatabaseContext dbcontext);
        public Task<Dto.ProductResponseList> GetProducts(DatabaseContext dbcontext);
        public Task<Dto.ProductResponse> GetProduct(Guid id, DatabaseContext dbcontext);
        public Task<Dto.ProductResponse> UpdateProduct(Dto.ProductRequest request, DatabaseContext dbcontext);
        public Task<Dto.ProductResponse> DeleteProduct(Guid id, DatabaseContext dbcontext);
        public Task<Dto.ProductResponseList> GetFilteredProducts(string categoryname, string productname, DatabaseContext dbcontext);
    }
}
