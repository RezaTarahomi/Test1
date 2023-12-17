using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class BaseAudit : BaseEntity
    {
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
