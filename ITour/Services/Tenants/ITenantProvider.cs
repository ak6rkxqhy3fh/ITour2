using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITour.Services.Tenants
{
    public interface ITenantProvider
    {
        Tenant Tenant { get; }
        Guid? GetTenantId();
        //Tenant GetTenant();
        Tenant GetTenantById(Guid? tenantId);
    }
}
