using Boxer.Data;
using Boxer.Management.Product.Data;
using Boxer.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boxer.Model.Dto;

namespace Boxer.Management.Service
{
    public  class ProductService: IProductService
    {

        private IConfiguration appConfig;
     
        public ProductService( IConfiguration config )
        {
            appConfig = config;
           
        }

        public String getId() {
            return "Prodcuct";
        }
        public async Task<Dto.ProductResponse> Save(Model.Dto.ProductRequest product, DatabaseContext dbcontext)  {

            try
            {
                var appSettings = appConfig.GetSection("AppSettings").Get<AppSettings>();
                Model.Product _product = new Model.Product()
                {
                    CategoryId = new Guid(product.categoryId),
                    Name = product.name,
                    Description = product.description,
                    Code = product.code,
                    Cost = product.cost.HasValue ? product.cost.Value : 0,
                    CreatedById = new Guid(product.userId),
                    ModifiedById = new Guid(product.userId)
                };

                dbcontext.product.Add(_product);
                 await dbcontext.SaveChangesAsync();

                return new Dto.ProductResponse( "", _product.Id.ToString(),_product.Name,_product.CategoryId.ToString(),_product.Description,_product.Code,_product.Cost );
            }
            catch (Exception ex)
            {
                return new Dto.ProductResponse(ex.Message, null,null,null,null,null, null);

            }

        }

        public async Task<Dto.ProductResponseList> GetProducts(DatabaseContext dbcontext)
        {
            try
            {

                List<Model.Product> categories = await dbcontext.product.ToListAsync();
                List<Dto.ProductResponse> productResponses = new List<Dto.ProductResponse>();
                categories.ForEach(product => {
                    productResponses.Add(new Dto.ProductResponse("",product.Id.ToString(),product.Name,product.CategoryId.ToString(),product.Description,product.Code, product.Cost));
                });

                return new Dto.ProductResponseList("", productResponses);
            }
            catch (Exception ex)
            {
                return new Dto.ProductResponseList(ex.Message, null);

            }
        }
        public async Task<Dto.ProductResponseList> GetFilteredProducts(string categoryname, string productname, DatabaseContext dbcontext)
        {
            try
            {
                if (categoryname == "-1") {
                    categoryname = string.Empty;
                }
                if (productname == "-1") {
                    productname = string.Empty;
                }

                List<ProductResponse> responses = new List<ProductResponse>();
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("@productName", System.Data.SqlDbType.NVarChar, 100) ;
                if (string.IsNullOrEmpty(productname))
                {
                    sqlParameters[0].Value = DBNull.Value;
                }
                else
                {
                    sqlParameters[0].Value = productname;
                }
                sqlParameters[1] = new SqlParameter("@categoryName", System.Data.SqlDbType.NVarChar, 100);
                if (string.IsNullOrEmpty(categoryname))
                {
                    sqlParameters[1].Value = DBNull.Value;
                }
                else {
                    sqlParameters[1].Value = categoryname;
                }
                
                var cmd = dbcontext.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "[dbo].[getProductCategories]";
                cmd.Parameters.Add(sqlParameters[0]);
                cmd.Parameters.Add(sqlParameters[1]);
                             
                cmd.CommandType=System.Data.CommandType.StoredProcedure;
                dbcontext.Database.GetDbConnection().Open();
                var result = await cmd.ExecuteReaderAsync();

                while (result.Read()) {
                    responses.Add(new ProductResponse(null, 
                        result["productId"].ToString(),
                        result["productName"].ToString(),
                        result["categoryId"].ToString(),
                        result["Description"].ToString(),
                         result["code"].ToString(),
                        (decimal?)result["cost"]));
                }

                // Run the sproc
                //ar reader = cmd.ExecuteReader();

                return new Dto.ProductResponseList("", responses);


            }
            catch (Exception ex)
            {
                return new Dto.ProductResponseList(ex.Message, null);

            }
        }
        public async Task<Dto.ProductResponse> DeleteProduct(Guid id,  DatabaseContext dbcontext)
        {
            try
            {

                var producToDelete = await dbcontext.product.FindAsync(id);
                if (producToDelete != null)
                {
                    dbcontext.product.Remove(producToDelete);
                    await dbcontext.SaveChangesAsync();

                }
                else {
                    return new Dto.ProductResponse("Product not found", null, null, null, null, null, null);
                }
              return  new Dto.ProductResponse("", null, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                return new Dto.ProductResponse(ex.Message, null, null, null, null, null, null);

            }
        }
        public async Task<Dto.ProductResponse> UpdateProduct(Dto.ProductRequest request, DatabaseContext dbcontext)
        {
            try
            {

                var producToUpdate = await dbcontext.product.FindAsync(new Guid(request.id));
                if (producToUpdate != null)
                {
                    producToUpdate.Name = request.name;
                    producToUpdate.CategoryId = new Guid(request.categoryId);   
                    producToUpdate.Description = request.description;
                    producToUpdate.ModifiedById = new Guid(request.userId);
                                        
                    await dbcontext.SaveChangesAsync();

                }
                else
                {
                    return new Dto.ProductResponse("Product not found", null, null, null, null, null, null);
                }
                return new Dto.ProductResponse(null,
                    producToUpdate.Id.ToString(),
                    producToUpdate.Name,
                    producToUpdate.CategoryId.ToString(),
                    producToUpdate.Description,producToUpdate.Code,producToUpdate.Cost);
            }
            catch (Exception ex)
            {
                return new Dto.ProductResponse(ex.Message, null, null, null, null, null, null);

            }
        }
        public async Task<Dto.ProductResponse> GetProduct(Guid id, DatabaseContext dbcontext)
        {
            try
            {

                var product = await dbcontext.product.FindAsync(id);
                if (product != null)
                {
                    return new Dto.ProductResponse("", product.Id.ToString(),
                        product.Name, 
                        product.CategoryId.ToString(), 
                        product.Description,product.Code, product.Cost );

                }
                else
                {
                    return new Dto.ProductResponse("Product not found", null, null, null, null, null, null);
                }
  
            }
            catch (Exception ex)
            {
                return new Dto.ProductResponse(ex.Message, null, null, null, null, null, null);

            }
        }

    }
}
