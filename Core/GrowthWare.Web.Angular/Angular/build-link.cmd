@cls
ng build gw-lib
cd dist/gw-lib
npm link
cd ../../
cd projects/gw-frontend
npm link gw-lib
cd ../../