param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath "GetZopfliPostBuildCmd.ps1")

$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value

if (!$currentPostBuildCmd.Contains($ZopfliPostBuildCmd))
{
    $project.Properties.Item("PostBuildEvent").Value += $ZopfliPostBuildCmd
}