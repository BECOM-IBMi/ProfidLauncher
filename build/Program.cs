using Build.Tasks;
using Cake.Core;
using Cake.Frosting;


public static class Program
{
    //public const string ARTIFACTS_PATH = "../artifacts";

    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<BuildContext>()
            .Run(args);
    }
}

public class BuildContext : FrostingContext
{
    public string ArtifactsPath { get; } = "../artifacts";

    public string PackagePath { get; } = "../package";

    public bool Delay { get; set; }

    public string Version { get; set; } = "";

    public BuildContext(ICakeContext context)
        : base(context)
    {
        Delay = context.Arguments.HasArgument("delay");
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(PackageTask))]
public class DefaultTask : FrostingTask
{
}