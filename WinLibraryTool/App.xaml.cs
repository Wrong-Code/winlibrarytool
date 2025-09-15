using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Dark.Net;
using Dark.Net.Wpf;

namespace WinLibraryTool
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		#region Properties
		private static Theme _theme;
		/// <summary>
		/// The theme used in the app.
		/// </summary>
		public static Theme Theme
		{
			get { return _theme; }
			set { _theme = value; }
		}
		#endregion

		public App()
		{
			// Initialize the theme manager
			App.Theme = Theme.Auto;
			DarkNet.Instance.SetCurrentProcessTheme(App.Theme);

			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			if (! Helpers.OSHelper.IsWindows7OrHigher())
			{
				ExitError("Sorry, this program is only useful on >= Windows 7.");
			}
		}

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

			// Register the light/dark skin resources with skin manager
			string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			new SkinManager().RegisterSkins(
				new Uri($"pack://application:,,,/{assemblyName};component/Resources/Skin.Light.xaml", UriKind.Absolute),
				new Uri($"pack://application:,,,/{assemblyName};component/Resources/Skin.Dark.xaml", UriKind.Absolute)
			);
		}

		private void ExitError(string message)
		{
			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
			{
				DialogType = WpfDialog.DialogType.Error
			};
			WpfDialog dialog = new WpfDialog(
				Helpers.AssemblyProperties.AssemblyTitle,
				message,
				options
			);
			dialog.ShowDialog();
			Environment.Exit(-1);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			DateTime now = DateTime.Now;
			string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			string logFileName = String.Format("{0}_{1}-{2:00}-{3:00}_{4:00}-{5:00}-{6:00}.log", assemblyName, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
			string logFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), assemblyName);
			string logFilePath = Path.Combine(logFolder, logFileName);

			if (! Directory.Exists(logFolder)) {
				Directory.CreateDirectory(logFolder);
			}
			File.WriteAllText(logFilePath, e.ExceptionObject.ToString());
			ExitError(
				String.Format(
@"Sorry, an unexpected error has occurred.
Information that will help solve this problem has been saved to:

{0}

Please send this problem report file to peter.g.horsley@gmail.com
for assistance.",
					logFilePath
				)
			);
		}

	}
}
