using System.IO;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace Cake.XamarinStudio.Commands
{
	public class AddConfigFileCommand : CakeCommandHandler
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
				monitor.BeginTask("Installing Cake configuration file", 1);
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
				monitor.BeginStep("Installing configuration file");
				var complete = CommandHelpers.DownloadFileToProject(
					Constants.ConfigDownloadPath,
					Path.Combine(targetPath, Constants.FileNames.ConfigFile),
						s => {
					var item = IdeApp.ProjectOperations.AddFilesToSolutionFolder(itemsFolder, new[] { s });
				});
			monitor.EndStep();
			if (complete)
			{
				monitor.ReportSuccess("Configuration file successfully installed");
			}
			else {
				monitor.ReportError("Error encountered while downloading configuration file");
			}
			monitor.EndTask();
		}
		}
	}
}