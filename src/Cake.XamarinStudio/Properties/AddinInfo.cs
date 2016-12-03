using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
	"Addin",
	Namespace = "Cake.XamarinStudio",
	Version = "0.1"
)]

[assembly: AddinName("Cake for Xamarin Studio")]
[assembly: AddinCategory("Build Tools")]
[assembly: AddinDescription("Adds support for the Cake build tool in Xamarin Studio. Includes support for syntax highlighting, new templates, snippets, and bootstrapping important Cake files.")]
[assembly: AddinAuthor("Cake Build")]
[assembly: AddinUrl("https://github.com/cake-build/cake-xs")]