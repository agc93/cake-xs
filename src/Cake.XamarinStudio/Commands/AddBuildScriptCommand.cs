namespace Cake.XamarinStudio.Commands
{
	public class AddBuildScriptCommand : CakeCommandHandler
	{
		protected override void Run()
		{
			var project = GetSelectedProject();
			if (project == null) return;
		}
	}
}
