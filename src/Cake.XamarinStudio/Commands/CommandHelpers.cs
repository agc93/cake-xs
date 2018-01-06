using System;
using System.Net;

namespace Cake.XamarinStudio.Commands
{
	internal static class CommandHelpers
	{
		internal static bool DownloadFileToProject(string downloadPath, string targetPath, Action<string> installCallback)
		{
			try
			{
				using (var wc = new WebClient())
				{
					wc.DownloadFile(downloadPath, targetPath);
					installCallback?.Invoke(targetPath);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
