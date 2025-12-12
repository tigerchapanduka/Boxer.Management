using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Model
{
    public class Dto
    {
        public record loginResponse(String? errorMessage,string?userId, String? token);
        public record ProductResponse ( String? errorMessage, String? id ,String? name, String? categoryId,String? description, string? code, Decimal? cost);
        public record AuthenticationRequest(String userName, String password);
        public record UserAccountRequest(String userName, String password);
        public record UserAccountResponse(String errorMessage, String userName, Guid? id);
        public record CategoryResponse(String? errorMessage, String? name, String? id);
        public record ProductRequest(String id, String? userId, String? name, String? categoryId, String? description, string? code, Decimal? cost);
        public record CategoryResponseList(string errorMessage,List<Dto.CategoryResponse>? categories);
        public record ProductResponseList(string errorMessage, List<Dto.ProductResponse>? products);
    }
}
