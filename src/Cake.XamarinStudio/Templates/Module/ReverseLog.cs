using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;


namespace ${Namespace}
{
    public sealed class ${EscapedIdentifier} : ICakeLog
	{
		private readonly IConsole _console;
		public Verbosity Verbosity { get; set; }

		public ${EscapedIdentifier}(IConsole console)
		{
			_console = console;
		}

		public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
		{
			var message = string.Format(format, args);
			_console.WriteLine(new string(message.Reverse().ToArray()));
		}
	}
}
