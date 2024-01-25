https://sandroroth.com/blog/angular-library/
Search: error TS5055: Cannot write file '.../public-api.d.ts' because it would overwrite input file.
    https://dev.to/krisplatis/relative-import-from-lib-s-secondary-entry-point-error-ts5055-cannot-write-file-x-d-ts-because-it-would-overwrite-input-file-25d6

Project structure

Below you can see the basic structure of the project that the CLI has generated for us (I omitted the irrelevant files). In the root there is the angular.json that configures the workspace and the package.json that contains all dependencies.

.
├── projects/
│   └── gw-lib/                          # (1)
|       ├── common/                      # (2)
|       │    ├─grouping1/
|       │    │  │ └─src/
|       │    │  │  └─ *.ts               # (3)
|       |    │  ├─index.ts               # (4)
|       |    │  ├─ ng-package.json       # (5)
|       |    │  ├─ public-api.ts         # (6)
|       │    ├─grouping2/
|       │    │  │ └─src/
|       │    │  │  └─ *.ts               # (3)
|       |    │  ├─index.ts               # (4)
|       |    │  ├─ ng-package.json       # (5)
|       |    │  └─ public-api.ts         # (6)
|       ├── core/                        # (2)
|       │    ├─grouping1/
|       │    │  │ └─src/
|       │    │  │  └─ *.ts               # (3)
|       |    │  ├─index.ts               # (4)
|       |    │  ├─ ng-package.json       # (5)
|       |    │  ├─ public-api.ts         # (6)
|       │    └─grouping2/
|       │       │ └─src/
|       │       │  └─ *.ts               # (3)
|       |       ├─index.ts               # (4)
|       |       ├─ ng-package.json       # (5)
|       |       └─ public-api.ts         # (6)
│       ├── ng-package.json              # (7)
│       ├── public-api.ts                # (8)
│       └── package.json                 # (4)
├── angular.json
└── package.json

Now let's go into detail of some files:

Library Code (1)
This folder contains the code of the library, currently a component and service. This is the main and only entry point of your library right now.

Public API (6)
The file contains the public API of the library. It exports all members that should be available to the outside world. That's the Angular component and service inside the src/lib/ folder.

ng-package.json (5)
The file contains the configuration for ng-packagr. It specifies the path of the build output and the entry file which points to src/public-api.ts.

package.json (4)
This is the package.json of your library (not to be confused with the package.json of your Angular workspace in the root folder). Here you specify the name, version and dependencies of your library.