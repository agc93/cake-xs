using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"Cake.XamarinStudio",
	Namespace = "Cake.XamarinStudio",
	Version = "0.2"
)]

[assembly: AddinName("Cake for MonoDevelop, Xamarin Studio and Visual for Studio Mac")]
[assembly: AddinCategory("Build Tools")]
[assembly: AddinDescription("Adds support for the Cake build tool in MonoDevelop, Xamarin Studio and Visual Studio for Mac. Includes support for syntax highlighting, new templates, snippets, and bootstrapping important Cake files.")]
[assembly: AddinAuthor("Cake Build")]
[assembly: AddinUrl("https://github.com/cake-build/cake-xs")]
