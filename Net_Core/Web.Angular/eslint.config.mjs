import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";


/** @type {import('eslint').Linter.Config[]} */
export default [
  {
    files: ["**/*.{js,mjs,cjs,ts}"]
  },
  {
    files: ["**/*.js"], 
    languageOptions: {sourceType: "script"}
  },
  {
    languageOptions: { globals: globals.browser }
  },
  { 
    rules: { 
      "no-duplicate-imports": "warn",
      "no-unused-vars": "warn",
      "@typescript-eslint/no-unused-vars": "warn"
    } 
  },
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
];