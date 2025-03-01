import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";


/** @type {import('eslint').Linter.Config[]} */
export default [
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
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
      "no-unused-vars": "off",
      "@typescript-eslint/no-unused-vars": "warn"
    } 
  }
];