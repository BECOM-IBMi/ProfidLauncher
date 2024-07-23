$assets = (get-location).path + "\assets"
$outpf = (get-location).path + "\output"
$package = (get-location).path + "\package"

$aip = $assets + "\Profid Launcher.aip"
$icon = $assets + "\favicon.ico"

cp $assets\*.ico $outpf\Assets\

$advinst = New-Object -ComObject AdvancedInstaller

$project = $advinst.LoadProject($aip)

$tag = git tag
$tag = $tag.Replace('v', '')
$tag = $tag.Substring(0, $tag.LastIndexOf('.') + 1)

$tag = $tag + $ENV:BUILD_NUMBER

$ver = dotnet-gitversion /output json /showvariable SemVer
echo $ver

$project.ProductDetails.Version = $ver
$project.ProductDetails.SetIcon($icon)

$pl = $project.FilesComponent.FindFileByPath("APPDIR\ProfidLauncher.exe")

$sc = $project.ShortcutsComponent.CreateFileShortcut($project.PredefinedFolders.Desktop, $pl)
$sc.Icon($icon)

$sc = $project.ShortcutsComponent.CreateFileShortcut($project.PredefinedFolders.ShortcutFolder, $pl)
$sc.Icon($icon)

$project.BuildComponent.Builds[0].OutputFolder = $package

$project.SaveAs($aip)

$project.Build()