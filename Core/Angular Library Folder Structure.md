I found the following links to be most helpful in understaning sub entries in an Angular library. 
    https://sandroroth.com/blog/angular-library
    https://tomastrajan.medium.com/the-best-way-to-architect-your-angular-libraries-87959301d3d3
        https://www.youtube.com/watch?v=2vHJ3_Om_gU

Project structure

Below you can see the basic structure of the project that the CLI has generated for us (I omitted the irrelevant files). In the root there is the angular.json that configures the workspace and the package.json that contains all dependencies.

<yourWorkSpace>
├── projects/
│   ├── <yourApplication>/
│   └── <yourLibrary>/                               # (1)
|       ├── Grouping1/                               # (2)
|       │    ├─ SubEntry1/                           # (3) import { SubEntry1Service } from '@mycompany/Grouping1/SubEntry1;
|       │    │   │ └─src/
|       │    │   │    └─ subentry1.service.ts        # (4)
|       |    │   ├─ index.ts                         # (5)
|       |    │   ├─ ng-package.json                  # (6)
|       |    │   ├─ public-api.ts                    # (7)
|       │    ├─ SubEntry2/                          import { SubEntry2Service } from '@mycompany/Grouping1/SubEntry2;
|       │    │   │ └─src/
|       │    │   │    └─ subentry2.service.ts        # (4)
|       |    │   ├─ index.ts                         # (5)
|       |    │   ├─ ng-package.json                  # (6)
|       |    │   └─ public-api.ts                    # (7)
|       ├── Grouping2/                               # (2)
|       │    ├─ SubEntry1/                          import { SubEntry1Service } from '@mycompany/Grouping2/SubEntry1;
|       │    │   │ └─src/
|       │    │   │    └─ subentry1.service.ts        # (4)
|       |    │   ├─ index.ts                         # (5)
|       |    │   ├─ ng-package.json                  # (6)
|       |    │   └─ public-api.ts                    # (7)
|       │    ├─ Grouping3/
|       │    │   └─ SubEntry1/                      import { SubEntry1Service } from '@mycompany/Grouping2/Grouping3/SubEntry1;
|       │    │       │ └─src/
|       │    │       │    └─ subentry1.service.ts    # (4)
|       |    │       ├─ index.ts                     # (5)
|       |    │       ├─ ng-package.json              # (6)
|       |    │       └─ public-api.ts                # (7)
|       │    └─ SubEntry2/                          import { SubEntry2Service } from '@mycompany/Grouping2/SubEntry2;
|       │        │ └─src/
|       │        │    └─ *.ts                        # (4)
|       |        ├─ index.ts                         # (5)
|       |        ├─ ng-package.json                  # (6)
|       |        └─ public-api.ts                    # (7)
│       ├── ng-package.json                          # (8)
│       ├── public-api.ts                            # (9)
│       └── package.json
├── angular.json
└── package.json

Now let's go into detail of some files:

Library Root (1)
    The root directory of the library.  The entry into the library or it's "name" is defined in <yourWorkSpace>\projects\tsconfig.json in the "paths" section and exposes your library for import statements.  Example if you would like to import statements to look like this:
        import { SubEntry1Service } from '@mycompany/Grouping1/SubEntry1;
    then you could use the following:
    "paths": {
      "@mycompany/*": [
        "./projects/yourLibrary/*",
        "./projects/yourLibrary"
      ],
      "@mycompany": [
        "./dist/yourLibrary/*",
        "./dist/yourLibrary"
      ]
    }

*.ts (4)

Grouping (2)
    Notice the the "Grouping" directors are empty of any files. Here is how we can give the illusion of having nested subentries ;-).  The empty directory will appear in the import statement and the sub entry is picked up where the ng-packagr find the next index.ts, ng-package.json and public-api.ts.
    Now is a good time to mention that the index.ts is not completely necessar.  If you like you can omit the file, but, you will need to change the name of public-api.ts to public_api.ts.  The ng-packagr will automatically find the public_api.ts

SubEntry Code (3)
    The directory that contains all of the sub entry files/folders.  At the root of the directory you can see three files index.ts, ng-package.json and public-api.ts and one directory "src".  I found this actually helps as you create sub entries it's "easier" to copy the three files from an existing sub entry into the new one and change what is export in the public-api.ts file.

*.ts (4)
    Any code files that make up the functionality of the sub entry.  In our example it is denoted as *.service.ts.

index.ts (5)
    The file contains the location of the public api export files.  As example:
        export * from './public-api';
    Again it's good to note that index.ts is not completely necessary.  It is possible to omit it and rename the public-api.ts to public_api.ts so it will be automatically picked up by the ng-packagr.

ng-package.json (6)
    The file contains configuration for the $schema and lib.  The lib shouldn't change but the $schema is a relitive path and should reflect the location to node_modules/ng-packagr/ng-package.schema.json as example:
        {
        "$schema": "../../../../node_modules/ng-packagr/ng-package.schema.json",
        "lib": {
            "entryFile": "public-api.ts"
        }
        }

public-api.ts (7)
    The Public API Surface of @mycompany/Grouping/SubEntry and exports any files that need to be exposed from the "src" directory of the SubEntry. As example
        /*
        * Public API Surface of @mycompany/<yourLibrary>/SubEntry1
        */

        export * from './src/subentry1.service';

ng-package.json (8)
    File contains the configuration for ng-packagr. It specifies the path of the build output and the entry file which points to public-api.ts.

public-api.ts (9)
    Unlike the other public-api.ts files this one doesn't specify any path insted it has:
        export const GROWTHWARE_GW_LIB = '@mycompany/<yourLibrary>';
    I should note you could also export default {}; with the same results

package.json (10)
    This is the package.json of your library (not to be confused with the package.json of your Angular workspace in the root folder). Here you specify the name, version and dependencies of your library.  The name is specified in the "name" property as example:
        {
        "name": "@mycompany",
        "version": "0.0.1",
        "peerDependencies": {
            "@angular/common": "^17.0.0",
            "@angular/core": "^17.0.0"
        },
        "dependencies": {
            "tslib": "^2.3.0"
        },
        "sideEffects": false
        }