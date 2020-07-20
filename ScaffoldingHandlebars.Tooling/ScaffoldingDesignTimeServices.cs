using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace ScaffoldingHandlebars.Tooling
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddHandlebarsScaffolding(options =>
            {
                options.EnableNullableReferenceTypes = true;
            });
        }
    }
}
