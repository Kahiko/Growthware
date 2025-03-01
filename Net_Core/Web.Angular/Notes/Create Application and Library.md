<style>
.right {
  position: absolute;
  right: 0px;
  width: 300px;
  border: 3px solid #73AD21;
  padding: 10px;
}    
</style>

## Directory Structure

In this example we'll be using Angular 18 along with multiple .Net projects. The directory structure will simular to the following:

[yourProjectsDirectory]<br/>
├── [businessLogic]<br/>
├── [webAPI]<br/>
├── [angularWorkSpace]<br/>
└── solution.sln<br/>
In my particular case I have 7 projets, 6 are .Net projects and 1 is the Angular workspace.<br/>

Net_Core<br/>
├── .vscode _ Important for the launch.json and task.json<br/>
├── BusinessLogic - .Net Project included in the solution<br/>
├── DataAccess - .Net Project included in the solution<br/>
├── DatabaseManager - .Net Project included in the solution<br/>
├── Framework - .Net Project included in the solution<br/>
├── Web.Angular _ This is the Angular workspace and not included in the solution<br/>
├── Web.Api - .Net Project included in the solution<br/>
├── Web.Support - .Net Project included in the solution<br/>
└── GrowthwareCore.sln<br/>

## Creating the Angular Workspace, Application and Library

1. Create an Angular Workspace In this case at the same level as all the other projects
    1. Disabling analytics is optional:<br/>
       ng analytics disable --global true
    2. Create the Angular workspace:<br/>
       ng new Web.Angular --create-application=false --skip-install<br/>
    3. Change directory:<br/>
       cd Web.Angular
2. Create the Application and Library
    1. Create the Application<br/>
       ng generate application gw-frontend --prefix gw-frontend --ssr false --style "scss" --skip-install<br/>
    2. Create the Library<br/>
       ng generate library gw-lib --prefix gw-core --skip-install<br/>

## Install any third party frameworks

Of course it is not necessary to install any third party frameworks. Here are some that I find useful.<br/>

1. ng add @angular/material --project gw-frontend<br/>
2. npm install @auth0/angular-jwt --save<br/>
3. ng add @angular-eslint/schematics<br/>
    1. Edit the angular.json file in the Web.Angular directory and add the following to the both of the"architect" sections of the "gw-frontend" and "gw-lib"</br>
<br/>

Path: projects/gw-frontend/architect<br/>
ADD: <br/>

```json
"lint": {
    "builder": "@angular-eslint/builder:lint",
    "options": {
        "lintFilePatterns": [
            "projects/gw-lib/**/*.ts",
            "projects/gw-lib/**/*.html"
        ]
    }
}
```
<br/>
Path: projects/gw-lib/architec<br/>
ADD: <br/>

```json
"lint": {
    "builder": "@angular-eslint/builder:lint",
    "options": {
        "lintFilePatterns": [
            "projects/gw-lib/**/*.ts",
            "projects/gw-lib/**/*.html"
        ]
    }
}
```
<br/>
4. Create the .eslint.config.mjs file<br/>
    1. npx eslint --init<br/>
5. Install ESLint extention (dbaeumer.vscode-eslint)
Create or update the settings.json<br/>
Path: .vscode<br/>
Add

```json
  "eslint.workingDirectories": [
    "Web.Angular", // The Angular workspace and location of the eslint.config.mjs
  ]
```
<br/>
   --npm install @microsoft/signalr<br/>

## Give the Library a name similar to "@angular" instead of the name passed in the "ng generate library" command

To name your entry point replace the "name" section in Web.Angular/projects/gw-lib/package.json. If you have built your library you will have package-lock.json file, you can either delete it (it will be recreated the next time you build) or you can edit it and change the name. I find that I delete the package-lock.json file often expecially when I change version numbers in package.json.<br/>

<table>
    <tr>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">/</td>
    </tr>
    <tr>
        <td style="text-align:right">FROM:</td>
        <td style="padding-left: 10px;">"name": "web.angular",</td>
    </tr>
    <tr>
        <td style="text-align:right">TO:</td>
        <td style="padding-left: 10px;">"name": "@growthware",</td>
    </tr>
</table><br/>
--Note: If you changed the entry point name, you should see that name when you build your library.  In this case "Building entry point '@growthware'"<br/>

At this point you should be able to build

<table>
    <tr>
        <td style="text-align:right;">Your library:</td>
        <td style="padding-left: 10px;">ng build gw-lib</td>
    </tr>
    <tr>
        <td style="text-align:right">Your frontend:</td>
        <td style="padding-left: 10px;">ng build gw-frontend</td>
    </tr>
    <tr>
        <td style="text-align:right">Serve the Angular project:</td>
        <td style="padding-left: 10px;">ng serve</td>
    </tr>
</table><br/>

The next 5 steps are optional but having the "projects\gw-lib\src\lib" directory when the library is named "gw-lib" and it's going to contain all of the source seems redundent.<br/>
So here are the steps to remove the "projects\gw-lib\src\lib" directory<br/> 1. Move public-api.ts file in the projects/gw-lib/src up one directory to projects/gw-lib. It is the lib directory and the public-api.ts<br/> 2. Edit the public-api.ts and replace it's contents with the example from the bottom<br/> 3. Delete the projects/gw-lib/src it's no longer needed<br/> 4. Replace the entryFile section in projects\gw-lib\ng-package.json (Serves as the entry point to the library)

<table>
    <tr>
        <td style="text-align:right;">path:</td>
        <td style="padding-left: 10px;">/</td>
    </tr>
    <tr>
        <td style="text-align:right">FROM:</td>
        <td style="padding-left: 10px;">"dest": "../../dist/gw-lib",</td>
    </tr>
    <tr>
        <td style="text-align:right">TO:</td>
        <td style="padding-left: 10px;">"dest": "../../dist/growthware/gw-lib",</td>
    </tr>
    <tr>
        <td style="text-align:right;">path:</td>
        <td style="padding-left: 10px;">lib</td>
    </tr>
    <tr>
        <td style="text-align:right">FROM:</td>
        <td style="padding-left: 10px;">"entryFile": "src/public-api.ts"</td>
    </tr>
    <tr>
        <td style="text-align:right">TO:</td>
        <td style="padding-left: 10px;">"entryFile": "public-api.ts"</td>
    </tr>
</table><br/>
	5. Replace the "sourceRoot" and "prefix" sections in Web.Angular\angular.json
<table>
    <tr>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">projects/gw-lib</td>
    </tr>
    <tr>
        <td style="text-align:right">FROM:</td>
        <td style="padding-left: 10px;">"sourceRoot": "projects/gw-lib/src",</td>
    </tr>
    <tr>
        <td style="text-align:right">TO:</td>
        <td style="padding-left: 10px;">"sourceRoot": "projects/gw-lib",</td>
    </tr>
</table><br/>

Replace the "paths" section in Web.Angular\tsconfig.json. Basically we need to add the two
parts of the path and change the name so other can reference the library as @growthware.
Make sure the directories match your structure!<br/><br/>
Path: compilerOptions/paths Change - <br/>
FROM: <br/>

```json
    "paths": {
        "gw-lib": [
        "./dist/gw-lib"
        ]
    },
```

    TO:

```json
    "paths": {
        "@growthware/*": [
            "./projects/gw-lib/*",
            "./projects/gw-lib"
        ],
        "@growthware": [
            "./dist/gw-lib/*",
            "./dist/gw-lib"
        ]
    },
```

## To host Angular Material fonts and icons locally

    1. npm install @fontsource/roboto material-icons --save
    2. Add the next two lines to the "scripts" section in Web.Angular\angular.json

<table>
    <tr>
        <td style="padding-left: 30px;"></td>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">projects/gw-frontend/architect/build/options/styles (you can search for "@angular/material/prebuilt-themes")</td>
    </tr>
    <tr>
        <td style="padding-left: 30px;"></td>
        <td style="text-align:right">*Add:</td>
        <td style="padding-left: 10px;">"node_modules/@fontsource/roboto/index.css",</td>
    </tr>
    <tr>
        <td style="padding-left: 30px;"></td>
        <td style="text-align:right">Add:</td>
        <td style="padding-left: 10px;">"node_modules/material-icons/iconfont/material-icons.css",</td>
    </tr>
</table><br/>
	2.a Optionaly you can do the same for the test build followint step 2, but the path is different<br/>
<table>
    <tr>
        <td style="padding-left: 30px;"></td>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">projects/gw-frontend/architect/test/options/styles</td>
    </tr>
</table>
	*Note: The @fontsource is based on the font you chose in step 1. In this case @fontsource/roboto

## To host both Angular and the Web.Api on the same machine (say during development) you'll need to do the following

    1. npm install run-script-os --save<br/>
          - run-script-os lets you use different commands in npm scripts depending on the operating system. Learn how to install, use, and customize it with examples and aliases.
    2. Create the aspnetcore-https.js (see... aspnetcore-https.js EXAMPLE) in the [angularWorkSpace] (Web.Angular)
    3. Add the aspnetcore-https to the "scripts" section in Web.Angular\package.json

<table>
    <tr>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">scripts</td>
    </tr>
    <tr>
        <td style="text-align:right; vertical-align:top">Add:</td>
        <td style="padding-left: 10px;">"prestart": "node aspnetcore-https",</td>
    </tr>
    <tr>
        <td style="text-align:right; vertical-align:top">Add:</td>
        <td style="padding-left: 10px;">"start:windows": "ng serve --host=127.0.0.1 --port 44455 --ssl --ssl-cert %APPDATA%\\ASP.NET\\https\\%npm_package_name%.pem --ssl-key %APPDATA%\\ASP.NET\\https\\%npm_package_name%.key",</td>
    </tr>
    <tr>
        <td style="text-align:right; vertical-align:top">Add:</td>
        <td style="padding-left: 10px;">"start:default": "ng serve --host=127.0.0.1 --port 44455 --ssl --ssl-cert $HOME/.aspnet/https/${npm_package_name}.pem --ssl-key $HOME/.aspnet/https/${npm_package_name}.key",</td>
    </tr>
    <tr>
        <td style="text-align:right">Path:</td>
        <td style="padding-left: 10px;">start</td>
    </tr>
    <tr>
        <td style="text-align:right">From:</td>
        <td style="padding-left: 10px;">"start": "ng serve",</td>
    </tr>
    <tr>
        <td style="text-align:right">To:</td>
        <td style="padding-left: 10px;">"start": "run-script-os",</td>
    </tr>
</table><br/>

    4. Create the proxy.conf.js (see... public-api.ts EXAMPLE) in the <baseAngular> (Web.Angular)<br/>
    5. Add the proxy.conf.js to the "development" section in Web.Angular\angular.json<br/>

<table>
    <tr>
        <td style="text-align:right;">Path:</td>
        <td style="padding-left: 10px;">projects/gw-frontend/architect/serve/configurations/development</td>
    </tr>
    <tr>
        <td style="text-align:right">Add:</td>
        <td style="padding-left: 10px;">"proxyConfig": "proxy.conf.js"</td>
    </tr>
</table><br/>

You should now be able to use npm to start the Angular project: npm start<br/>
The Angular project will be available at http://127.0.0.1:44455 and it should automatically detect the correct OS and use the correct start:windows or start:default configuration.

## File Examples

### public-api.ts EXAMPLE

```typescript
/*
 * Public API Surface of @growthware
 */

export const GROWTHWARE_GW_LIB = "@growthware/gw-lib";
```

### aspnetcore-https.js EXAMPLE

```javascript
// This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate
const fs = require("fs");
const spawn = require("child_process").spawn;
const path = require("path");

const baseFolder = process.env.APPDATA !== undefined && process.env.APPDATA !== "" ? `${process.env.APPDATA}/ASP.NET/https` : `${process.env.HOME}/.aspnet/https`;

const certificateArg = process.argv.map((arg) => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : process.env.npm_package_name;

if (!certificateName) {
    console.error("Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.");
    process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
    spawn("dotnet", ["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"], { stdio: "inherit" }).on("exit", (code) => process.exit(code));
}
```

proxy.conf.js **<em>Notes</em>**:<br/>
The "context:" exclues the https call from Angular and allows the call to pass through to the API. The context is determined by the class name of the controller(s). For example a controller class is named "public class weatherforecastController : ControllerBase" then the context is "/weatherforecast". Note that the context property is a JSON array object where each element is a string separated by a comma.<br/>
You can find the "ASPNETCORE_ENVIRONMENT" environment variable in the launch.json file. Typically I will remove any of the .vscode directories except the one at the root of my project directories (the same level as the solution.sln file). Having the .vscode directory at the root allows for multiple "configuration" that you can "run" or "debug".<br/>

### proxy.conf.js EXAMPLE

```javascript
const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT ? `https://127.0.0.1:${env.ASPNETCORE_HTTPS_PORT}` : env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(";")[0] : "http://127.0.0.1:35360";

const PROXY_CONFIG = [
    {
        context: ["/weatherforecast"],
        target: target,
        secure: false,
        headers: {
            Connection: "Keep-Alive",
        },
    },
];

module.exports = PROXY_CONFIG;
```

### launch.json EXAMPLE

```json
{
    // Use IntelliSense to find out which attributes exist for C# debugging
    // Use hover for the description of the existing attributes
    // For further information visit https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md
    "version": "0.2.0",
    "configurations": [
        {
            "name": "PWA Chrome",
            "type": "chrome",
            "request": "launch",
            // For .net 6.0 + this port needs to match Properties\launch.json:applicationUrl
            // otherwise the SPA will not launch
            "url": "https://127.0.0.1:44455",
            "webRoot": "${workspaceFolder}/Web.Angular",
            "sourceMaps": true,
            // "trace": true,
            "sourceMapPathOverrides": {
                "webpack:///./*": "${webRoot}/*"
            },
            "preLaunchTask": "Task: Start NodeJS",
            "postDebugTask": "Task: Stop NodeJS"
        },
        {
            "name": "Web.Api",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "Task: Api-Build",
            // If you have changed target frameworks, make sure to update the program path.
            "program": "${workspaceFolder}/Web.Api/bin/Debug/net9.0/GrowthWare.Web.Api.dll",
            "args": [],
            "cwd": "${workspaceFolder}/Web.Api",
            "stopAtEntry": false,
            "internalConsoleOptions": "openOnSessionStart",
            "launchBrowser": {
                "enabled": false
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    ],
    "compounds": [
        {
            "name": "1.) API + Chrome",
            "configurations": ["Web.Api", "PWA Chrome"]
        }
    ]
}
```

### tasks.json EXAMPLE

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "Task: Api-Build",
            "command": "dotnet",
            "type": "process",
            "args": ["build", "${workspaceFolder}/Web.Api/GrowthWare.Web.Api.csproj", "/property:GenerateFullPaths=true", "/consoleloggerparameters:NoSummary"],
            "problemMatcher": "$msCompile"
        },
        {
            "label": "Task: Start NodeJS",
            "type": "shell",
            "isBackground": true,
            "options": {
                "cwd": "${workspaceFolder}/Web.Angular"
            },
            "command": "npm",
            "args": ["start"],
            "presentation": {
                "reveal": "always",
                "panel": "new"
            },
            "problemMatcher": [
                {
                    "pattern": [
                        {
                            "regexp": ".",
                            "file": 1,
                            "line": 1,
                            "column": 1,
                            "message": 1
                        }
                    ],
                    "background": {
                        "activeOnStart": true,
                        "beginsPattern": { "regexp": "." },
                        "endsPattern": { "regexp": "." }
                    }
                }
            ]
        },
        {
            "label": "Task: Stop NodeJS",
            "command": "echo ${input:terminate}",
            "type": "shell"
        }
    ]
}
```

### .eslintrc.json EXAMPLE

```json
{
    "root": true,
    "overrides": [
        {
            "files": ["*.ts", "*.html"],
            "parser": "@typescript-eslint/parser",
            "parserOptions": {
                "project": "./tsconfig.json"
            },
            "plugins": ["@angular-eslint", "@typescript-eslint"],
            "extends": ["eslint:recommended", "plugin:@angular-eslint/recommended", "plugin:@typescript-eslint/recommended", "plugin:import/errors", "plugin:import/warnings"],
            "rules": {
                "no-console": "warn",
                "@typescript-eslint/no-explicit-any": "off",
                "import/no-unresolved": "off"
            }
        }
    ]
}
```

### eslint.config.mjs

```javascript
import { fixupConfigRules, fixupPluginRules } from "@eslint/compat";
import angularEslint from "@angular-eslint/eslint-plugin";
import typescriptEslint from "@typescript-eslint/eslint-plugin";
import tsParser from "@typescript-eslint/parser";
import path from "node:path";
import { fileURLToPath } from "node:url";
import js from "@eslint/js";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all,
});

export default [
    ...fixupConfigRules(compat.extends("eslint:recommended", "plugin:@angular-eslint/recommended", "plugin:@typescript-eslint/recommended", "plugin:import/errors", "plugin:import/warnings")).map((config) => ({
        ...config,
        files: ["**/*.ts", "**/*.html"],
    })),
    {
        files: ["**/*.ts", "**/*.html"],

        plugins: {
            "@angular-eslint": fixupPluginRules(angularEslint),
            "@typescript-eslint": fixupPluginRules(typescriptEslint),
        },

        languageOptions: {
            parser: tsParser,
            ecmaVersion: 5,
            sourceType: "script",

            parserOptions: {
                project: "./tsconfig.json",
            },
        },

        rules: {
            "no-console": "warn",
            "@typescript-eslint/no-explicit-any": "off",
            "import/no-unresolved": "off",
        },
        ignores: ["dist/*, node_modules/*"],
    },
];
```
