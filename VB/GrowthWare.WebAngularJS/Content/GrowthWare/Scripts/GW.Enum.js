// Namespaces
if (typeof GW == "undefined" || !GW) {
	window.GW = {
		name: 'Growthware Core Web',
		version: '1.0.0.0'
	};
}

if (typeof GW.Enum == "undefined" || !GW.Enum) {
	GW.Enum = {
		name: 'Growthware Core Web Enum objects javascript',
		version: '1.0.0.0'
	};
}

GW.Enum = {
	FunctionType: {
		Module: 1,
		Security: 2,
		Menu_Item: 3,
		Calendar: 4,
		File_Manager: 5
	},

	NavigationType: {
		Horizontal: 1,
		Vertical: 2,
		Hierarchical: 3
	},
}