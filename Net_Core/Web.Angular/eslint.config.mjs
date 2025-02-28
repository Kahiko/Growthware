// import { fixupConfigRules, fixupPluginRules } from "@eslint/compat";
// import angularEslint from "@angular-eslint/eslint-plugin";
// import typescriptEslint from "@typescript-eslint/eslint-plugin";
// import tsParser from "@typescript-eslint/parser";
// import path from "node:path";
// import { fileURLToPath } from "node:url";
// import js from "@eslint/js";
// import { FlatCompat } from "@eslint/eslintrc";

// const __filename = fileURLToPath(import.meta.url);
// const __dirname = path.dirname(__filename);
// const compat = new FlatCompat({
//     baseDirectory: __dirname,
//     recommendedConfig: js.configs.recommended,
//     allConfig: js.configs.all
// });

// export default [...fixupConfigRules(compat.extends(
//     "eslint:recommended",
//     "plugin:@angular-eslint/recommended",
//     "plugin:@typescript-eslint/recommended",
//     "plugin:import/errors",
//     "plugin:import/warnings",
// )).map(config => ({
//     ...config,
//     files: ["**/*.ts", "**/*.html"],
// })), {
//     files: ["**/*.ts", "**/*.html"],

//     plugins: {
//         "@angular-eslint": fixupPluginRules(angularEslint),
//         "@typescript-eslint": fixupPluginRules(typescriptEslint),
//     },

//     languageOptions: {
//         parser: tsParser,
//         ecmaVersion: 5,
//         sourceType: "script",

//         parserOptions: {
//             project: "./tsconfig.json",
//         },
//     },

//     rules: {
//         "no-console": "warn",
//         "@typescript-eslint/no-explicit-any": "off",
//         "import/no-unresolved": "off",
//     },
//     ignores: [
//         "dist/*", 
//         "node_modules/*", 
//         "*.spec.ts"
//     ],
// }];

import tseslint from "@typescript-eslint/eslint-plugin";
import tsparser from "@typescript-eslint/parser";
import prettierPlugin from "eslint-plugin-prettier";
import prettierConfig from "eslint-config-prettier";

export default [
  {
    files: ["**/*.ts"],

    languageOptions: {
      parser: tsparser,
      sourceType: "module",
    },

    plugins: {
      "@typescript-eslint": tseslint,
      prettier: prettierPlugin,
    },

    rules: {
      ...tseslint.configs.recommended.rules,
      ...prettierConfig.rules,
      "@typescript-eslint/no-unused-vars": "warn",
      "no-console": "warn",
      "semi": ["error", "always"],
      "quotes": ["error", "double"],
      "prettier/prettier": "error",
    },
  },
];