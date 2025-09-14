using System;
using System.Diagnostics;
using System.Windows.Input;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class ApplyChangesCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public ApplyChangesCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			CheckBox mirrorCheckBox = new CheckBox();
			string mirrorRoot = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Libraries");
			mirrorCheckBox.Content = String.Format("Create a mirror of all libraries (using symbolic links) in {0}", mirrorRoot);
			if (System.IO.Directory.Exists(mirrorRoot))
			{
				mirrorCheckBox.Content += "\n(Existing directory will be erased.)";
			}
			mirrorCheckBox.IsChecked = false;

			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
			{
				DialogType = WpfDialog.DialogType.Warning,
				CustomContent = mirrorCheckBox,
				PossibleResponses = new WpfDialog.UserResponses(new string[] { "Proceed", "Cancel" }, 1),
				TitleBarIcon = ((Window) parameter).Icon
			};

			WpfDialog dialog = new WpfDialog(
				Helpers.AssemblyProperties.AssemblyTitle,
@"Your existing library structure will now be backed-up, and the
ones defined in this tool will be created.

If any problem occurs during creation of the new libraries,
the backed-up copies will be restored.",
				options
			);
			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();

			if (dialog.UserResponse.Equals("Proceed", StringComparison.CurrentCultureIgnoreCase))
			{
				bool appliedOK = false;

				try
				{
					Mouse.OverrideCursor = Cursors.Wait;
					_viewModel.ApplyChanges(mirrorCheckBox.IsChecked.Value);
					Mouse.OverrideCursor = Cursors.Arrow;
					appliedOK = true;
				}
				catch (System.Exception ex)
				{
					Mouse.OverrideCursor = null; // revert cursor

					WpfDialog.WpfDialogOptions errorOptions = new WpfDialog.WpfDialogOptions
					{
						DialogType = WpfDialog.DialogType.Error,
						PossibleResponses = new WpfDialog.UserResponses(new string[] { "Bugger" }),
						TitleBarIcon = ((Window) parameter).Icon
					};

					WpfDialog resultDialog = new WpfDialog(
						Helpers.AssemblyProperties.AssemblyTitle,
						"An error occurred whilst configuring the libraries.\n\n" + ex.Message,
						errorOptions
					);
					resultDialog.Owner = (Window)parameter;
					resultDialog.ShowDialog();
				}

				if (appliedOK)
				{
					CheckBox openCheckBox = new CheckBox();
					openCheckBox.Content = "Show me in Windows Explorer";
					openCheckBox.IsChecked = true;

					WpfDialog.WpfDialogOptions successOptions = new WpfDialog.WpfDialogOptions
					{
						DialogType = WpfDialog.DialogType.Information,
						PossibleResponses = new WpfDialog.UserResponses(new string[] { "OK" }),
						CustomContent = openCheckBox,
						TitleBarIcon = ((Window) parameter).Icon
					};

					WpfDialog resultDialog = new WpfDialog(
						Helpers.AssemblyProperties.AssemblyTitle,
						"Your new libraries have been created successfully.",
						successOptions
					);
					resultDialog.Owner = (Window)parameter;
					resultDialog.ShowDialog();

					if (openCheckBox.IsChecked.Value)
					{
						Process.Start("explorer");
					}
				}
			}
		}

		public bool CanExecute(object parameter)
		{
			return _viewModel.CanApplyChanges();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
