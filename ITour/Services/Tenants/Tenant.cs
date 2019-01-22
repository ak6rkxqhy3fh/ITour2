using System;

namespace ITour.Services.Tenants
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
    }
}
