---
trigger: always_on
---

# General Code Style & Formatting
- Use English for all code and documentation.
- Always declare the type of each variable and function (parameters and return value).
- Avoid using any.
- Create necessary types.
- Use JSDoc to document public classes and methods.
- Don't leave blank lines within a function.
- One export per file.

# Naming Conventions
- `m_` prefix: Module/class-wide variables in PascalCase (e.g., `m_ServiceName`, `m_DependencyInjection`)
  - Used for variables that are shared within the module/component scope
  - Follows the pattern: `m_PascalCase`

- `m` prefix: Local method variables that represent something specific
  - Followed by a descriptive name in PascalCase
  - Follows the pattern: `mPascalCase`
  - Examples:
    - `mCounter`: A counter variable
    - `mUserList`: A list of users
    - `mIsLoading`: A boolean flag for loading state

- Function parameters: Use standard camelCase
  - No prefix needed for parameters
  - Examples:
    ```javascript
    function processUserData(userData, options) {
        // ...
    }
    ```

- `mRet` prefix: Used for variables that hold return values
  - `mRetVal`: The standard return value variable within a method
  - `mRetSvc`: Factory function that returns a service instance
  - `mRetCtrl`: Factory function that returns a controller instance
  - `mRetDir`: Factory function that returns a directive definition
  - Indicates the function's role in the component lifecycle

- Centralized name references:
  - `ServiceNames`: For service names
  - `DirectiveNames`: For directive names
  - `ControllerNames`: For controller names
  - Prevents typos and provides autocompletion

# Functions & Logic
- Keep functions short and single-purpose (<20 lines).
- Avoid deeply nested blocks by:
- Using early returns.
- Extracting logic into utility functions.
- Use higher-order functions (map, filter, reduce) to simplify logic.
- Use arrow functions for simple cases (<3 instructions), named functions otherwise.
- Use default parameter values instead of null/undefined checks.
- Use RO-RO (Receive Object, Return Object) for passing and returning multiple parameters.

# Data Handling
- Avoid excessive use of primitive types; encapsulate data in composite types.
- Avoid placing validation inside functions—use classes with internal validation instead.
- Prefer immutability for data:
- Use readonly for immutable properties.
- Use as const for literals that never change.