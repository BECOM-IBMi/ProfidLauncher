using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;

namespace Build.Tasks;

[TaskName("Compile")]
[IsDependentOn(typeof(RestoreTask))]
public sealed class CompileTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context
            .DotNetBuild("../ProfidLauncher.sln", new DotNetBuildSettings
            {
                NoRestore = true
            });
    }
}

[TaskName("Test")]
[IsDependentOn(typeof(CompileTask))]
public sealed class TestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetTest("../ProfidLauncher.sln", new DotNetTestSettings { NoRestore = true });
    }
}