using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace WinLibraryTool.Commands
{
	public class HelpCommand : ICommand
	{
		public HelpCommand()
		{
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			TextBlock infoText = new TextBlock();
			string mirrorRoot = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Libraries");
			infoText.Inlines.Add(
$@"This tool provides the following features not available in Windows:

  • Add network (UNC or mapped drive) and any other un-indexed folders to libraries.
  • Backup library configuration, such that a saved set of libraries can be instantly
     restored at any point.
  • Create a mirror of all libraries (using symbolic links) in {mirrorRoot}
  • Change a library's icon.

"
			);

			Run webAddress = new Run("http://zornsoftware.codenature.info");
			Hyperlink link = new Hyperlink(webAddress);
			link.NavigateUri = new Uri(webAddress.Text);
			link.Click += new RoutedEventHandler(link_Click);
			infoText.Inlines.Add(link);

			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
			{
				DialogType = WpfDialog.DialogType.Information,
				DialogIcon = ((Window) parameter).Icon,
				PossibleResponses = new WpfDialog.UserResponses(new string[] { "OK" }),
				TitleBarIcon = ((Window) parameter).Icon,
				CustomContent = infoText
			};

			WpfDialog dialog = new WpfDialog(
				Helpers.AssemblyProperties.AssemblyTitle, 
				String.Format("{0} v{1}\n{2}\n{3}", 
					Helpers.AssemblyProperties.AssemblyTitle, 
					Helpers.AssemblyProperties.AssemblyVersion, 
					Helpers.AssemblyProperties.AssemblyDescription, 
					Helpers.AssemblyProperties.AssemblyCopyright
				), 
				options
			);

			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();
		}

		void link_Click(object sender, RoutedEventArgs e)
		{
			// open URL
			Hyperlink source = sender as Hyperlink;
			if (source != null)
			{
				Process.Start(source.NavigateUri.ToString());
			}
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			// I intentionally left these empty because
			// this command never raises the event, and
			// not using the WeakEvent pattern here can
			// cause memory leaks.  WeakEvent pattern is
			// not simple to implement, so why bother.
			add { }
			remove { }
		}

		#endregion
	}
}
