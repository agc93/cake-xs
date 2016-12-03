#addin nuget:?package=Cake.Xamarin

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutionPath = File("./src/Cake.XamarinStudio.sln");
var solution = ParseSolution(solutionPath);
var projects = solution.Projects;
var projectPaths = projects.Select(p => p.Path.GetDirectory());
var artifacts = "./dist/";

///////////////////////////////////////////////////////////////////////////////
// BUILD VARIABLES
///////////////////////////////////////////////////////////////////////////////

var buildSystem = BuildSystem;
var IsMainCakeVsRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake-xs", buildSystem.AppVeyor.Environment.Repository.Name);
var IsMainCakeVsBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.AppVeyor.Environment.Repository.Branch);
var IsBuildTagged = buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
	// Executed BEFORE the first task.
	Information("Running tasks...");
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
	// Clean solution directories.
	foreach(var path in projectPaths)
	{
		Information("Cleaning {0}", path);
		CleanDirectories(path + "/**/bin/" + configuration);
		CleanDirectories(path + "/**/obj/" + configuration);
	}
	Information("Cleaning common files...");
	CleanDirectory(artifacts);
});

Task("Restore")
	.Does(() =>
{
	// Restore all NuGet packages.
	Information("Restoring solution...");
	NuGetRestore(solutionPath);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.Does(() =>
{
	Information("Building solution...");
	MSBuild(solutionPath, settings =>
		settings
			.WithProperty("TreatWarningsAsErrors","true")
			.SetVerbosity(Verbosity.Quiet)
			.WithTarget("Build")
			.SetConfiguration(configuration));
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() => 
{
	Information("Not packaging addin");
	/* Information("Packaging addin...");
	MDToolSetup.Pack("./src/Cake.XamarinStudio/bin/" + configuration + "/Cake.XamarinStudio.dll", artifacts); */
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
