using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Model
{
    public class Product : EntityBase
    {
        public required String Name { get; set; }
        public required Guid CategoryId { get; set; }
        public String Description { get; set; }
        public String Code { get; set; }
        public Decimal  Cost { get; set; }
    }
        
}
