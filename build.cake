#addin "nuget:?package=Cake.Xamarin"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./Cake.XamarinStudio.sln");
var solution = ParseSolution(solutionPath);
var projectNames = new string[] { "Cake.XamarinStudio" };
var artifacts = "./dist/";

///////////////////////////////////////////////////////////////////////////////
// BUILD VARIABLES
///////////////////////////////////////////////////////////////////////////////

var buildSystem = BuildSystem;
var IsMainCakeVsRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake-xs",
	buildSystem.AppVeyor.Environment.Repository.Name);
var IsMainCakeVsBranch = StringComparer.OrdinalIgnoreCase.Equals("master",
	buildSystem.AppVeyor.Environment.Repository.Branch);
var IsBuildTagged = buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
	if(IsRunningOnWindows())
	{
		if (FileExists(@"C:\Program Files (x86)\Xamarin Studio\bin\mdtool.exe"))
		{
			ctx.Tools.RegisterFile(@"C:\Program Files (x86)\Xamarin Studio\bin\mdtool.exe");
		}
	}
	else
	{
		if (FileExists(@"\Applications\Visual Studio.app\Contents\MacOS\vstool"))
		{
			ctx.Tools.RegisterFile(@"\Applications\Visual Studio.app\Contents\MacOS\vstool");
		}
	}
});

Teardown(ctx =>
{
	// Executed AFTER the last task.
	Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	CleanDirectory(artifacts);
	foreach(var projectName in projectNames)
    {
        CleanDirectories(string.Format("./src/{0}/{1}/bin/**", configuration, projectName));
        CleanDirectories(string.Format("./src/{0}/{1}/obj/**", configuration, projectName));
    }
});

Task("Restore")
	.Does(() =>
{
	NuGetRestore(solutionPath);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	foreach(var projectName in projectNames)
	{
		MSBuild(string.Format("./src/{0}/{0}.csproj", projectName), settings =>
			settings
				.WithProperty("TreatWarningsAsErrors","true")
				.SetVerbosity(Verbosity.Verbose)
				.WithTarget("Build")
				.SetConfiguration(configuration));
	}
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
{
	if(IsRunningOnWindows())
	{
		MDToolSetup.Pack("./src/Cake.XamarinStudio/bin/" + configuration + "/net461/Cake.XamarinStudio.dll", artifacts);
	}
	else
	{
		VSToolSetup.Pack("./src/Cake.XamarinStudio/bin/" + configuration + "/net461/Cake.XamarinStudio.dll", artifacts);
	}
});

Task("Publish-Extension")
    .IsDependentOn("Package")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(() => IsMainCakeVsRepo)
	.WithCriteria(() => FileExists(artifacts + "Cake.XamarinStudio.mpack"))
    .Does(() =>
{
    AppVeyor.UploadArtifact(artifacts + "Cake.XamarinStudio.mpack");
});

Task("Default")
	.IsDependentOn("Package");

Task("AppVeyor")
    .IsDependentOn("Publish-Extension");

RunTarget(target);
