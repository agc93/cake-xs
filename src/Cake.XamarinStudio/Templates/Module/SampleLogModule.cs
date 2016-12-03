using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;

[assembly: CakeModule(typeof(${Namespace}.${EscapedIdentifier}))]

namespace ${Namespace}
{
    public class ${EscapedIdentifier} : ICakeModule
	{
		public void Register(ICakeContainerRegistrar registrar)
		{
			registrar.RegisterType<ReverseLog>().As<ICakeLog>().Singleton();
		}
	}
}
