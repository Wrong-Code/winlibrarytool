using System;
using System.Windows;
using System.Windows.Input;
using WinLibraryTool.UserControls;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class EditLibraryCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public EditLibraryCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			LibraryPropertiesControl userControl = new LibraryPropertiesControl(_viewModel.CurrentLibrary);
			WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
			{
				DialogType = WpfDialog.DialogType.Information,
				PossibleResponses = new WpfDialog.UserResponses(new string[] { "Close" }, 0),
				CustomContent = userControl,
				TitleBarIcon = ((Window) parameter).Icon
			};
			WpfDialog dialog = new WpfDialog(
				String.Format("{0}  |  Edit library", Helpers.AssemblyProperties.AssemblyTitle),
				"You can add local and network folders to this library.",
				options
			);
			dialog.Owner = (Window)parameter;
			dialog.ShowDialog();
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanEditLibrary());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
