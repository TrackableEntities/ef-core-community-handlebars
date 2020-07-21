using EntityFrameworkCore.Scaffolding.Handlebars;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace ScaffoldingHandlebars.Tooling
{
    public class MyHbsCSharpEntityTypeGenerator : HbsCSharpEntityTypeGenerator
    {
        private readonly IOptions<HandlebarsScaffoldingOptions> _options;

        public MyHbsCSharpEntityTypeGenerator(
            ICSharpHelper cSharpHelper, 
            IEntityTypeTemplateService entityTypeTemplateService, 
            IEntityTypeTransformationService entityTypeTransformationService, 
            IOptions<HandlebarsScaffoldingOptions> options) 
            : base(cSharpHelper, entityTypeTemplateService, entityTypeTransformationService, options)
        {
            _options = options;
        }

        protected override void GenerateProperties(IEntityType entityType)
        {
            var properties = new List<Dictionary<string, object>>();

#pragma warning disable EF1001 // Internal EF Core API usage.
            foreach (var property in entityType.GetProperties().OrderBy(p => p.GetColumnOrdinal()))
#pragma warning restore EF1001 // Internal EF Core API usage.
            {
                PropertyAnnotationsData = new List<Dictionary<string, object>>();

                if (UseDataAnnotations)
                {
                    GeneratePropertyDataAnnotations(property);
                }

                var propertyType = CSharpHelper.Reference(property.ClrType);
                if (_options?.Value?.EnableNullableReferenceTypes == true
                    && property.IsNullable
                    && !propertyType.EndsWith("?"))
                {
                    propertyType += "?";
                }
                properties.Add(new Dictionary<string, object>
                {
                    { "property-type", propertyType },
                    { "property-name", property.Name },
                    { "property-annotations",  PropertyAnnotationsData },
                    { "property-comment", property.GetComment() },
                    { "property-isnullable", property.IsNullable },
                    { "nullable-reference-types", _options?.Value?.EnableNullableReferenceTypes == true },

                    // Add new item to template data
                    { "property-isprimarykey", property.IsPrimaryKey() }
                });
            }

            var transformedProperties = EntityTypeTransformationService.TransformProperties(properties);

            // Add to transformed properties
            for (int i = 0; i < transformedProperties.Count ; i++)
            {
                transformedProperties[i].Add("property-isprimarykey", properties[i]["property-isprimarykey"]);
            }

            TemplateData.Add("properties", transformedProperties);
        }
    }
}
