# run this first: Set-ExecutionPolicy -ExecutionPolicy Unrestricted
$paths =  "c:\temp\create_tests_services.cmd", "c:\temp\create_tests_other.cmd"
foreach($filePath in $paths)
{
    if (Test-Path $filePath) {
        Remove-Item $filePath -verbose
    } else {
        Write-Host  $filePath + " doesn't exits"
    }
}
$angularSrc = "D:\Development\Growthware\Core\Web.Angular\Angular\projects\"
$configFile ="D:\Development\Growthware\Core\Generate_Spec_Files\ngentest.config.js"
$includeTypes = "*.class.ts", "*.component.ts", "*.directive.ts", "*.guard.ts", "*.interceptor.ts", "*.pipe.ts", "*.service.ts"
$fileNames = Get-ChildItem -Path $angularSrc -Recurse -Include ($includeTypes)
foreach ($f in $fileNames){
    $command = 'npx ngentest "' + $f.FullName + '" -s -f --framework="karma" -c "' + $configFile + '"'
    if ($f.FullName -Match ".service.ts") {
        Add-Content -Path "c:\temp\create_tests_services.cmd" -Value $command
    } else {
        Add-Content -Path "c:\temp\create_tests_other.cmd" -Value $command
    }
}