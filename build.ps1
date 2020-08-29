$location = Get-Location
try {
    Set-Location $PSScriptRoot
    $buildFile = "./.vscode/build.ps1"
    $propsPath = (
        "../JSSoft.Library/Directory.Build.props",
        "../JSSoft.Library.Commands/Directory.Build.props",
        "../JSSoft.ModernUI.Framework/Directory.Build.props",
        "./Directory.Build.props"
    ) | ForEach-Object { "`"$_`"" }
    $propsPath = $propsPath -join ","
    $solutionPath = "./JSSoft.Font.sln"
    Invoke-WebRequest -Uri "https://raw.githubusercontent.com/s2quake/build/master/build.ps1" -OutFile $buildFile
    Invoke-Expression "$buildFile $solutionPath $propsPath $args"
}
finally {
    Remove-Item ./.vscode/build.ps1
    Set-Location $location
}
