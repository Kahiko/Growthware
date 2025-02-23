module.exports = {
	parser: '@typescript-eslint/parser', // Specify the ESLint parser for TypeScript
	extends: [
		'plugin:@typescript-eslint/recommended', // Use recommended rules from @typescript-eslint
	],
	rules: {
		'indent': ['error', 'tab'], // Enforce tabs for indentation
		// Disable all other rules
		'no-unused-vars': 'off',
		'no-console': 'off',
		'semi': 'off',
		'no-extra-semi': 'off',
		'prefer-const': 'off',
		'quotes': 'off',
		'@angular-eslint/component-class-suffix': 'off',
		'@typescript-eslint/prefer-const': 'off',
		'@typescript-eslint/no-unused-vars': 'off',
		'@typescript-eslint/no-explicit-any': 'off',
	},
	overrides: [
		{
			files: ['*.spec.ts'],                       // Specify the files to ignore
			rules: {
				// Disable all rules for .spec.ts files
				'indent': 'off',
				'@typescript-eslint/no-unused-vars': 'off',
				'@typescript-eslint/prefer-const': 'off',
				// Add other rules to disable as needed
			},
		},
	],
};
