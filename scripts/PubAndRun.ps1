"Publishing:"
DotNet publish | Out-Null
"Deploying:"
$cwd = Get-Location 

Move-Item -Force  .\usage\Floatingman.CommandLine.Application\bin\Debug\net5.0\publish\*.* f:\tmp\cmd\ | Out-Null
Move-Item -Force  .\usage\Floatingman.CommandLine.Command\bin\Debug\net5.0\publish\*.* f:\tmp\cmd\plugins\GenerateHexArray | Out-Null

Set-Location f:\tmp\cmd\
"Running: [Floatingman.CommandLine.Application GenerateHexArray -U 2 -V 2 --radius 10]"
"~"
.\Floatingman.CommandLine.Application.exe GenerateHexArray -U 2 -V 2 --radius 10.0
"~"
Set-Location $cwd 
