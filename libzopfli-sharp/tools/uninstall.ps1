param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath "GetZopfliPostBuildCmd.ps1")

try
{
	$currentPostBuildCmd = $project.Properties.Item("PostBuildEvent").Value
	$project.Properties.Item("PostBuildEvent").Value = $currentPostBuildCmd.Replace($ZopfliPostBuildCmd, '')
}
catch { }