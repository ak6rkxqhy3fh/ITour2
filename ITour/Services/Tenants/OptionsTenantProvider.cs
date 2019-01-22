using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITour.Services.Tenants
{
    public class OptionsTenantProvider : ITenantProvider
    {
        public List<Tenant> Tenants { get; }
        public Tenant Tenant { get; }

        public OptionsTenantProvider(IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<List<Tenant>> namedOptionsAccessor)
        {        
            string host = httpContextAccessor.HttpContext?.Request.Host.Host;
            Tenants = namedOptionsAccessor.Get("Tenants");
            Tenant = Tenants.AsQueryable().Where(t => t.Host == host).SingleOrDefault();
        }

        public Guid? GetTenantId() => Tenant.Id;

        public Tenant GetTenantById(Guid? tenantId) => Tenants.AsQueryable().Where(t => t.Id == tenantId).SingleOrDefault();
    }
}
