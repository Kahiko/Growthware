---
trigger: always_on
---

# C# Coding Standards

## 1. Naming Conventions

### Variables and Fields
- Use m_PascalCase for class scoped fields
- Use `mPascalCase` for private fields: `private int mCount;`
- Use `camelCase` for parameters
- Use `PascalCase` for public fields and properties: `public int TotalCount { get; set; }`
- Use camelcase for private methods
- Prefix interface names with `I`: `public interface IRepository`
- Suffix async methods with `Async`: `public async Task GetDataAsync()`

### Constants
- Use `PascalCase` for constants: `public const int MaxRetryCount = 3;`
- Group related constants in a static class: `public static class ConfigurationKeys`

### Methods
- Use PascalCase for method names: `public void CalculateTotal()`
- Use verb-noun pairs: `GetUserById()`, `SaveChangesAsync()`
- Boolean methods should ask a question: `IsValid()`, `HasPermission()`

## 2. Code Organization

### Class Structure
```csharp
public class ExampleClass
{
    // Constants
    private const string DefaultName = "Default";
    
    // Fields
    private readonly ILogger _logger;
    
    // Properties
    public string Name { get; set; }
    
    // Constructor
    public ExampleClass(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    // Public methods
    public void DoWork()
    {
        // Implementation
    }
    
    // Private methods
    private string FormatName(string name)
    {
        return string.IsNullOrEmpty(name) ? DefaultName : name.Trim();
    }
}
## 3. Language Usage

### Async/Await
- **Always await async methods**  
  ```csharp
  await DoWorkAsync();  // Correct
  DoWorkAsync().Wait(); // Avoid - blocks thread
  ```
- **Use `ValueTask<T>` for high-performance scenarios**  
  ```csharp
  public ValueTask<int> GetValueAsync() => ValueTask.FromResult(42);
  ```
- **Avoid `async void`** (except for event handlers)  
  ```csharp
  // Only for event handlers
  private async void Button_Click(object sender, EventArgs e) => await DoWork();
  // Otherwise use
  public async Task ProcessAsync() { /* ... */ }
  ```

### Null Checking
- **Null-conditional operator**: `var name = user?.Name;`
- **Null-coalescing operator**: `var name = user.Name ?? "Unknown";`
- **Pattern matching**: `if (user is not null) { /* ... */ }`

### String Handling
- **String interpolation**: `$"Hello, {name}!"`
- **StringBuilder for large concatenations**:
  ```csharp
  var sb = new StringBuilder();
  foreach (var item in items) sb.Append(item);
  var result = sb.ToString();
  ```
- **Use `nameof()`**: `throw new ArgumentNullException(nameof(user));`
## 4. Error Handling

### Exception Handling
- **Catch specific exceptions** instead of the base `Exception` class
  ```csharp
  // Good
  try { /* ... */ }
  catch (FileNotFoundException ex) { /* ... */ }
  
  // Avoid
  try { /* ... */ }
  catch (Exception ex) { /* ... */ }
  ```

- **Use exception filters** for cleaner conditional handling
  ```csharp
  try { /* ... */ }
  catch (Exception ex) when (ex is InvalidOperationException || 
                           ex is ArgumentException)
  {
      // Handle specific exceptions
  }
  ```

- **Provide meaningful exception messages** with context
  ```csharp
  if (user == null)
      throw new ArgumentNullException(nameof(user), "User cannot be null");
  ```

### Logging
- **Use structured logging** with named placeholders
  ```csharp
  _logger.LogInformation("Processing user {UserId} with role {UserRole}", 
      userId, userRole);
  ```

- **Log exceptions with context**
  ```csharp
  try 
  {
      // Code that might fail
  }
  catch (Exception ex)
  {
      _logger.LogError(ex, "Failed to process user {UserId}", userId);
      throw;
  }
  ```
## 5. Performance Considerations

### Collections
- Prefer `List<T>` over `ArrayList`
- Use `Span<T>` or `Memory<T>` for high-performance scenarios
- Prefer `Any()` over `Count > 0` for `IEnumerable<T>`

### LINQ
- Use method syntax for complex queries
- Consider `AsParallel()` for CPU-bound operations
- Be aware of deferred execution

## 6. Testing

### Naming
- Follow the pattern: `MethodName_StateUnderTest_ExpectedBehavior`
- Example: `CalculateTotal_NegativeInput_ThrowsException`

### Structure
- Use the Arrange-Act-Assert pattern
- Keep tests focused and independent
- Use test data builders for complex objects

## 7. Documentation

### XML Documentation
```csharp
/// <summary>
/// Calculates the total price including tax.
/// </summary>
/// <param name="price">The base price before tax.</param>
/// <param name="taxRate">The tax rate as a decimal (e.g., 0.08 for 8%).</param>
/// <returns>The total price including tax.</returns>
/// <exception cref="ArgumentOutOfRangeException">Thrown when price is negative.</exception>
public decimal CalculateTotal(decimal price, decimal taxRate)
{
    if (price < 0)
        throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative");
        
    return price * (1 + taxRate);
}

## 8. EditorConfig

Consider adding an `.editorconfig` file to enforce these standards:

```ini
# Core EditorConfig options
root = true

# All files
[*]
indent_style = space
indent_size = 4
insert_final_newline = true
charset = utf-8-bom
end_of_line = crlf

# C# files
[*.cs]
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
csharp_new_line_before_members_in_object_initializers = true
csharp_new_line_before_members_in_anonymous_types = true
csharp_new_line_between_query_expression_clauses = true

## 9. Code Analysis

Enable code analysis in your project file by adding these settings to your `.csproj` file:

```xml
<PropertyGroup>
  <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  <AnalysisLevel>latest</AnalysisLevel>
  <EnableNETAnalyzers>true</EnableNETAnalyzers>
</PropertyGroup>

## 10. Versioning

### Semantic Versioning (SemVer)
Follow the `MAJOR.MINOR.PATCH` versioning scheme:
- **MAJOR**: Breaking changes
- **MINOR**: Backwards-compatible features
- **PATCH**: Backwards-compatible bug fixes

### Project File Configuration
Set the version in your `.csproj` file:
```xml
<PropertyGroup>
  <VersionPrefix>1.0.0</VersionPrefix>
  <!-- Or for more detailed versioning -->
  <Version>1.0.0</Version>
  <AssemblyVersion>1.0.0.0</AssemblyVersion>
  <FileVersion>1.0.0.0</FileVersion>
</PropertyGroup>
