using System.IO;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace Cake.XamarinStudio.Commands
{
	public class AddBootstrappersCommand : CakeCommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			var project = GetSelectedProject();
			info.Enabled = !(project == null);
		}

		protected override void Run()
		{
			using (var monitor = CreateProgressMonitor("Installing Cake configuration file"))
			{
				monitor.BeginTask("Installing Cake configuration file", 3);
				var solution = GetCurrentSolution();
				monitor.BeginStep("Getting solution details");
				var targetPath = solution.BaseDirectory.FullPath;
				var itemsFolder = solution.DefaultSolutionFolder;
				var build = solution.FindSolutionItem("build.cake");
				if (build != null)
				{
					itemsFolder = build.ParentFolder;
					targetPath = build.ItemDirectory;
				}
				monitor.EndStep();
				monitor.BeginStep("Installing PowerShell bootstrapper");
				var posh = CommandHelpers.DownloadFileToProject(
					Constants.PowerShellDownloadPath,
					Path.Combine(targetPath, Constants.FileNames.PoshScript),
						s =>
						{
							var item = IdeApp.ProjectOperations.AddFilesToSolutionFolder(itemsFolder, new[] { s });
						});
				monitor.EndStep();
				monitor.BeginStep("Installing Bash bootstrapper");
				var bash = CommandHelpers.DownloadFileToProject(
					Constants.BashDownloadPath,
					Path.Combine(targetPath, Constants.FileNames.BashScript),
						s =>
						{
							var item = IdeApp.ProjectOperations.AddFilesToSolutionFolder(itemsFolder, new[] { s });
						});
				if (bash && posh)
				{
					monitor.ReportSuccess("Bootstrapper scripts successfully installed");
				}
				else {
					monitor.ReportError("Error encountered while downloading bootstrappers");
				}
				monitor.EndTask();
			}
		}
	}
}
