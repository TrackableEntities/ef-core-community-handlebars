# EF Core Community Standup: Scaffolding with Handlebars Templates

Demos for [July 2020 EF Core Community Standup](https://youtu.be/6Ux7EpgiWXE).

> **Prerequisites**:
> - [NorthwindSlim](https://github.com/TrackableEntities/northwind-slim) sample database.
>   - Create **NorthwindSlim** database.
>   - Run **NorthwindSlim.sql** script.
> - [EF Core Power Tools](https://github.com/ErikEJ/EFCorePowerTools/wiki).

> **References**:
>- [Handlebars Language Guide](https://handlebarsjs.com/guide).
> - [Entity Framework Core Tools Reference](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).
> - [Entity Framework Core Scaffolding with Handlebars](https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars).

> **Note**: When using Visual Studio for simple use cases, the **EF Core Power Tools** offers a _graphical user interface_ and the ability to directly target a _.NET Standard_ library.
> 
> For _advanced features_, such as Handlebars helpers and transformers, custom template data, nullable reference types and schema folders, it is necessary to use the **EF Core CLI**, which requires a "tooling" project that is a **.NET Core** library. You also need to add a `ScaffoldingDesignTimeServices` class to the tooling project.

> **Note**: Handlebars scaffolding includes additional features _not shown_ in this demo. For details refer to the project [ReadMe](https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars/blob/master/README.md).
> - Add Handlebars [block helpers](https://handlebarsjs.com/guide/block-helpers.html#basic-blocks).
>  - **Exclude tables** from code generation.
>  - Add custom **template data** for use in Handlebars templates.
>  - Enable **schema folders** to place generated entities in folders according to database schemas.
>  - Translate table and column descriptions to XML **comments**.
>  - **Embed** Handlebars templates into a separate **assembly**.
>
> Many of these features were **contributed** by members of the community. To submit an issue or pull request, please refer to the project [Contributing Guidelines](https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars/blob/master/.github/CONTRIBUTING.md).

### 1. EF Core Power Tools with Handlbars Templates (C#)

> **Note**: The easiest way to reverse engineer entities from an existing database is to use a Visual Studio extension called the [Entity Framework Code Power Tools](https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools), which allow you to customize generated code using Handlebars templates.

1. Create a **.NET Standard** library project with a **.Entities** suffix.
   - Because EF Core Power Tools is a Visual Studio extension, you can use it on a **.NET Standard** Library.
2. Add a **Data Connection** to the Server Explorer.
   - Server name: `(localdb)\MsSqlLocalDb`
   - Select `NorthwindSlim` database.
3. Right-click on **ScaffoldingHandlebars.Entities** project in the Solution Explorer, select EF Core Power Tools, Reverse Engineer.
   - Choose the NorthwindSlim data connection.
   - Select all tables.
   - Check: Customize code using Handlebars templates.
    ![efcpt-rev-eng](images/efcpt-rev-eng.png)
4. Modify Handlebars templates to control code generation.
   - Modify the **CSharpEntityType/Class.hbs** file to derive from `EntityBase`.
   - Add `EntityBase` class to the project.
   - Modify **CSharpEntityType/Partials/Properties.hbs** to change `ICollection` to `List`.
   - Update **CSharpEntityType/Partials/Constructor.hbs** change `HashSet` to `List`.
   - Add a parameterless constructor to **CSharpDbContext/Partials/Constructor.hbs**.
   - Rerun the EF Core Power Tools.

### 2. EF Core Power Tools with Handlbars Templates (TypeScript)

> **Note**: The EF Core Power Tools also allow you to reverse engineer [TypeScript](https://www.typescriptlang.org/) entities from a database and customize them using Handlebars templates.

1. Create a **.NET Standard** library project with a **.TypeScript** suffix.
2. Right-click on **ScaffoldingHandlebars.Entities** project in the Solution Explorer, select EF Core Power Tools, Reverse Engineer.
   - Choose the NorthwindSlim data connection.
   - Select all tables.
   - What to generate: **Entity Types only**
   - Check: Customize code using Handlebars templates.
   - Select **TypeScript** as the language.

    ![efcpt-rev-eng-ts](images/efcpt-rev-eng-ts.png)

### 3. EF Core CLI with Handlebars Templates

> **Note**: First we will create a separate .NET Standard **.Data** project for the generated `DbContext` class, so that the **.Entities** project will not require any dependency on EF Core and can be references by interfaces that are ignorant of persistence concerns.

> Because the [EF Core CLI](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet) requires the .NET Core runtime, it is helpful to create a **.Tooling** .NET Core Library project, which will generate the context and entities in the .NET Standard **.Data** and **.Entities** projects.

1. Install EF Core Global Tool
    ```bash
    dotnet tool install --global dotnet-ef
    ```
   - You may need to update the tools to the latest available version.
    ```bash
    dotnet tool update --global dotnet-ef
    ```
2. Delete the CodeTemplates, Contexts and Models folders from the **.Entities** project.
3. Create a **.NET Standard** project with a **.Data** suffix.
   - This is where the `DbContext` class will be generated.
   - Reference the **.Entities** project from the **.Data** project.
   - Add EF Core SQL Server package.
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    ```
4. Create a **.NET Core** library project with a **.Tooling** suffix.
   - The tools need the _.NET Core runtime_ to execute, so a .NET Standard project cannot be used by the tooling.
5. Reference the **.Entities** project from the **.Tooling** project.
6. Add EF Core Design, Scaffolding.Handlebars packages.
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package EntityFrameworkCore.Scaffolding.Handlebars
    ```
7. Add a `ScaffoldingDesignTimeServices` class that implements `IDesignTimeServices`.
    ```csharp
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddHandlebarsScaffolding();
        }
    }
    ```
8. Run the `dotnet ef dbcontext scaffold` command.
   - Make sure you are in the **.Tooling** project directory.
   - Specify connection string, EF Core SQL Server provider, and target project.
   - `DbContext` can be generated in the **.Data** project by specifying the `--context-dir` argument.
   - Entities can be generated in a separate project by specifying the `--project` argument and specifying the **.Entities** project.
    ```bash
    dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=NorthwindSlim; Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c NorthwindSlimContext --context-dir ../ScaffoldingHandlebars.Data/Contexts --project ../ScaffoldingHandlebars.Entities --force
    ```

### 4. Enable Nullable Reference Types

> **Note**: Handlebars scaffolding supports generation of entities with [nullable reference type semantics](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-reference-types#nullable-references-and-static-analysis).

1. Upgrade both the **.Data** and **.Entities** projects to .NET Standard **version 2.1**.
   - Either edit the **.csproj** files directory or select Target Framework on the _Application_ tab of the project properties page in Visual Studio.
2. Enable **Nullable** for both the **.Data** and **.Entities** projects.
   - Either edit the **.csproj** files directory or select Target Framework on the _Build_ tab of the project properties page in Visual Studio.
3. Update `services.AddHandlebarsScaffolding` in `ScaffoldingDesignTimeServices` in the **.Tooling** project to enable nullable reference types.
   ```csharp
   services.AddHandlebarsScaffolding(options =>
      options.EnableNullableReferenceTypes = true
   );
   ```
4. Run the `dotnet ef dbcontext scaffold` command.
   - Use the same command as shown above.
   - Notice that _optional_ properties for reference types are set to [nullable](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references) by appending a `?` to the type.
   - Notice that _required_ properties are set using the [null forgiving operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving), which eliminates compiler warnings.

### 5. Handlebars Helpers

> **Note**: Handlebars Helpers allow you to inject custom text into generated entities using a [C# tuple](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples) with a `context` parameter that includes template data.

1. Update `ScaffoldingDesignTimeServices.ConfigureDesignTimeServices` in the **.Tooling** project by adding a call to `services.AddHandlebarsHelpers`.
   ```csharp
   services.AddHandlebarsHelpers(("my-helper", (writer, context, parameters) =>
      writer.Write($"// My Handlebars Helper: {context["class"]}")
   ));
   ```
2. Update **CSharpEntityType/Class.hbs** to add `{{my-helper}}`
3. Run the `dotnet ef dbcontext scaffold` command from above.
4. To inspect the `context` variable, add a breakpoint inside the lambda statement, then add the following line of code in `ConfigureDesignTimeServices`.
   ```csharp
   Debugger.Launch();
   ```
5. When the scaffolding command is run you will be prompted to select an instance of Visual Studio for attaching the JIT Debugger.
   - When the breakpoint is hit you can inspect the content of the `context` parameter.

### 6. Handlebars Transformers

> **Note**: Handlebars Transformers are a lightweight mechanism for transforming entities and their members. For example, this can allow you to change a property from a `string` to an `enum`.

> In this example we will change the `Country` property of `Employee` from `string` to an enum called `Country` with values that match data in the `Employee` and tables.

1. Add a `Country` enum to the **.Entities** project.
   ```csharp
   public enum Country
   {
      UK = 1,
      USA = 2
   }
   ```
2. Add a call to `services.AddHandlebarsTransformers` to `ConfigureDesignTimeServices`, in which you pass a property transformer.
   ```csharp
   services.AddHandlebarsTransformers(propertyTransformer: e =>
      e.PropertyName == nameof(Country)
         ? new EntityPropertyInfo(nameof(Country), nameof(Country), e.PropertyIsNullable)
         : new EntityPropertyInfo(e.PropertyType, e.PropertyName, e.PropertyIsNullable)
   );
   ```
3. To test this add a .NET Core console app with a **.ConsoleClient** suffix.
   - Add **Microsoft.EntityFrameworkCore.SqlServer** package.
   - Reference both **.Data** and **.Entities** projects.
   - Add code to `Program.Main` for displaying name, city and country for `Employees`.
   ```csharp
   using (var context = new NorthwindSlimContext())
   {
      var employees = context.Employee.Select(e =>
         new { Name = $"{e.FirstName} {e.LastName}", e.City, e.Country });
      foreach (var e in employees)
      {
         Console.WriteLine($"{e.Name} {e.City} {e.Country} ");
      }
   }  
   ```
   - Press Ctrl+F5 to run the console client.
   - Notice the `InvalidCastException` converting `string` to `int`.
4. To fix the error add a **NorthwindSlimContextPartial.cs** file with a partial `NorthwindSlimContext` class that implements the partial `OnModelCreatingPartial` method.
   - Specify `HasConversion` for the `Employee.Country` property, converting the `Country` property between `string` and the `Country` enum.
   ```csharp
   namespace ScaffoldingHandlebars.Entities
   {
      public partial class NorthwindSlimContext
      {
         partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
         {
               modelBuilder.Entity<Employee>()
                  .Property(e => e.Country)
                  .HasConversion(
                     v => v.ToString(),
                     v => (Country)Enum.Parse(typeof(Country), v));
         }
      }
   }
   ```
5. Re-run the console client. The exception will no longer appear.

### 7. Full Control: Extend Handlebars Generators

> **Note**: To take full control of context and entity generation, you can extend `HbsCSharpDbContextGenerator` and `HbsCSharpEntityTypeGenerator`, overriding select virtual methods. Then register your custom generators in `ScaffoldingDesignTimeServices.ConfigureDesignTimeServices`.

> For example, you may want to add `property-isprimarykey` to the template data in order to insert some code or a comment.

1. Add a `MyHbsCSharpEntityTypeGenerator` to the **.Tooling** project.
   - Extend `HbsCSharpEntityTypeGenerator`.
   - Override `GenerateProperties`.
   - Copy code from the base `GenerateProperties` method.
   - Add code that inserts `property-isprimarykey` into the template data.
   ```csharp
   protected override void GenerateProperties(IEntityType entityType)
   {
      var properties = new List<Dictionary<string, object>>();
      foreach (var property in entityType.GetProperties().OrderBy(p => p.GetColumnOrdinal()))
      {
         // Code elided for clarity
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
   ```
2. Register `MyHbsCSharpEntityTypeGenerator` in `ScaffoldingDesignTimeServices.ConfigureDesignTimeServices`.
   ```csharp
   services.AddSingleton<ICSharpEntityTypeGenerator, MyHbsCSharpEntityTypeGenerator>();
   ```
3. Update **CSharpEntityType/Partials/Properties.hbs** to add `property-isprimarykey`.
   ```handlebars
   {{#if property-isprimarykey}} // Primary Key{{/if}}
   ```
4. Run the `dotnet ef dbcontext scaffold` command from above.
