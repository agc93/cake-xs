using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace ${Namespace}
{
	[CakeAliasCategory("Sample")]
	public static class AddinAliases
	{
		[CakeMethodAlias]
		public static void Hello(this ICakeContext ctx, string name)
		{
			ctx.Log.Information("Hello " + name);
		}
	}
}
