using Cake.Core.IO;
using Cake.Frosting;
using Cake.SevenZip;
using Cake.SevenZip.Commands;
using System.IO;
using System.Linq;

namespace Build.Tasks;

[TaskName("Package")]
[IsDependentOn(typeof(PublishTask))]
public sealed class PackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        var d = new DirectoryInfo(context.ArtifactsPath);
        var files = d.GetFiles().Select(x => new FilePath(x.FullName));

        var fileCollection = new FilePathCollection(files);

        context.SevenZip(new SevenZipSettings
        {
            Command = new AddCommand
            {
                Directories = new DirectoryPathCollection { new DirectoryPath($"{context.ArtifactsPath}/Assets"), new DirectoryPath($"{context.ArtifactsPath}/runtimes") },
                Archive = new FilePath($"{context.PackagePath}/v{context.Version}.zip"),
                Files = fileCollection
            }
        });
    }
}

//[TaskName("WriteVersionFile")]
//[IsDependentOn(typeof(PackageTask))]
//public sealed class WriteVersionFileTask : FrostingTask<BuildContext>
//{
//    public override void Run(BuildContext context)
//    {
//        var version = new VersionFileModel
//        {
//            Current = context.Version,
//        };

//        var str = JsonSerializer.Serialize(version, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

//        File.WriteAllText($"{context.PackagePath}/profidlauncher.v1.json", str);
//    }
//}