using System;
using System.Windows;
using System.Windows.Input;
using WinLibraryTool.UserControls;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class CreateLibraryCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public CreateLibraryCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			bool created = false;
			bool cancelled = false;
			string windowTitle = String.Format("{0}  |  Add library", Helpers.AssemblyProperties.AssemblyTitle);
			while (!cancelled && !created)
			{
				UserInputControl userControl = new UserInputControl("Name:", "");
				WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
				{
					DialogType = WpfDialog.DialogType.Question,
					PossibleResponses = new WpfDialog.UserResponses(new string[] { "Create", "Cancel" }, 0),
					CustomContent = userControl,
					TitleBarIcon = ((Window) parameter).Icon
				};
				WpfDialog dialog = new WpfDialog(
					windowTitle,
					"Enter the name of the new library.",
					options
				) {
					Owner = (Window) parameter
				};
				dialog.ShowDialog();

				if (dialog.UserResponse.Equals("Create", StringComparison.CurrentCultureIgnoreCase))
				{
					if (userControl.InputText.Length > 0)
					{
						_viewModel.CreateLibrary(userControl.InputText);
						created = true;
					}
					else
					{
						WpfDialog.WpfDialogOptions errorOptions = new WpfDialog.WpfDialogOptions
						{
							DialogType = WpfDialog.DialogType.Error,
							TitleBarIcon = ((Window) parameter).Icon
						};
						WpfDialog errorDialog = new WpfDialog(
							windowTitle,
							"You must enter a name for the new library.",
							errorOptions
						);
						errorDialog.Owner = (Window)parameter;
						errorDialog.ShowDialog();
					}
				}
				else
				{
					cancelled = true;
				}
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
