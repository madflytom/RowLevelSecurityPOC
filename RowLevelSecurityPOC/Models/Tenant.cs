using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RowLevelSecurityPOC.Models
{
    public class Tenant
    {
        public Guid TenantId { get; set; }
        public Guid APIKey { get; set; }
        public string TenantName { get; set; }
    }
}
