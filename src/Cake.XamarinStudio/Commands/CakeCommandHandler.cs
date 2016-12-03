using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace Cake.XamarinStudio.Commands
{
	public class CakeCommandHandler : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			var project = GetSelectedProject();
			info.Enabled = !(project == null);
		}

		protected DotNetProject GetSelectedProject()
		{
			return IdeApp.ProjectOperations.CurrentSelectedProject as DotNetProject;
		}

		protected Solution GetCurrentSolution()
		{
			return IdeApp.ProjectOperations.CurrentSelectedSolution;
		}

		protected ProgressMonitor CreateProgressMonitor(string title)
		{
			var consoleMonitor = IdeApp.Workbench.ProgressMonitors.GetOutputProgressMonitor(
				"CakeBuild",
				GettextCatalog.GetString("Cake Build"),
				Stock.Console,
				false,
				true);

			Pad pad = IdeApp.Workbench.ProgressMonitors.GetPadForMonitor(consoleMonitor);

			ProgressMonitor statusMonitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor(
				title,
				Stock.StatusSolutionOperation,
				true,
				false,
				false,
				pad);

			return new AggregatedProgressMonitor(consoleMonitor, statusMonitor);
		}
	}
}
