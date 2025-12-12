using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.Model
{
    public abstract class EntityBase
    {
        public Guid Id { get;  set; }
        public DateTime CreatedDate { get; private set; } = DateTime.Now;
        public DateTime ModifiedDate { get; private set; } = DateTime.Now;
        public Guid ModifiedById { get;  set; }
        public Guid CreatedById { get; set; }
        public void UpdateLastModified(Guid modifiedbyId)
        {
            ModifiedDate = DateTime.Now;
            ModifiedById = modifiedbyId;
        }
    }
}
