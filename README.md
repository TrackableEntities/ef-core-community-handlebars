# EF Core Community Standup: Scaffolding with Handlebars Templates

Demos for July 2020 EF Core Community Standup

> **Prerequisites**:
> - [NorthwindSlim](https://github.com/TrackableEntities/northwind-slim) sample database.
>   - Create **NorthwindSlim** database.
>   - Run **NorthwindSlim.sql** script.
> - [EF Core Power Tools](https://github.com/ErikEJ/EFCorePowerTools/wiki).

> **References**:
> - [Entity Framework Core Tools Reference](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).
> - [Entity Framework Core Scaffolding with Handlebars](https://github.com/TrackableEntities/EntityFrameworkCore.Scaffolding.Handlebars).

> **Note**: When using Visual Studio for simple use cases, the **EF Core Power Tools** offers a _graphical user interface_ and the ability to directly target a _.NET Standard_ library.
> 
> For _advanced features_, such as Handlebars helpers and transformers, custom template data, nullable reference types and schema folders, it is necessary to use the **EF Core CLI**, which requires a "tooling" project that is a **.NET Core** library. You also need to add a `ScaffoldingDesignTimeServices` class to the tooling project.

### 1. EF Core Power Tools with Handlbars Templates (C#)

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
   - Remove `connectionstring-warning` from **CSharpDbContext/Partials/DbOnConfiguring.hbs**
   - Rerun the EF Core Power Tools.

### 2. EF Core Power Tools with Handlbars Templates (TypeScript)

1. Create a **.NET Standard** library project with a **.TypeScript** suffix.
2. Right-click on **ScaffoldingHandlebars.Entities** project in the Solution Explorer, select EF Core Power Tools, Reverse Engineer.
   - Choose the NorthwindSlim data connection.
   - Select all tables.
   - What to generate: **Entity Types only**
   - Check: Customize code using Handlebars templates.
   - Select **TypeScript** as the language.
    ![efcpt-rev-eng-ts](images/efcpt-rev-eng-ts.png)

### 3. EF Core CLI with Handlebars Templates

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
6. Add EF Core Design, SQL Server and Scaffolding.Handlebars packages.
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
   - Notive that _optional_ properties for reference types are set to [nullable](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references) by appending a `?` to the type.
   - Notice that _required_ properties are set using the [null forgiving operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving), which eliminates compiler warnings.

### 5. Handlebars Helpers

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

> **Handlebars Transformers** are a lightweight mechanism for transforming entities and their members. For example, this can allow you to change a property from a `string` to an `enum`.
> 
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
5. Re-run the console client.