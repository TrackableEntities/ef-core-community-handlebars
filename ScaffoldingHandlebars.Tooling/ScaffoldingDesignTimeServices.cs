using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using ScaffoldingHandlebars.Entities;
using System.Diagnostics;

namespace ScaffoldingHandlebars.Tooling
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            // Launch debugger
            //Debugger.Launch();

            // Add Handlebars scaffolding
            services.AddHandlebarsScaffolding(options =>
                options.EnableNullableReferenceTypes = true
            );

            // Add Handlebars helper
            services.AddHandlebarsHelpers(("my-helper", (writer, context, parameters) =>
                writer.Write($"// My Handlebars Helper: {context["class"]}")
            ));

            // Add Handlebars transformer
            services.AddHandlebarsTransformers(propertyTransformer: e =>
                e.PropertyName == nameof(Country)
                    ? new EntityPropertyInfo(nameof(Country), nameof(Country), e.PropertyIsNullable)
                    : new EntityPropertyInfo(e.PropertyType, e.PropertyName, e.PropertyIsNullable)
            );
        }
    }
}
