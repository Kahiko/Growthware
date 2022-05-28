@CLS
rmdir node_modules\gw-lib /s/q
rmdir dist\gw-lib /s/q
call ng build gw-lib
rem cd dist\gw-lib
rem call npm pack
rem cd ..\..\
