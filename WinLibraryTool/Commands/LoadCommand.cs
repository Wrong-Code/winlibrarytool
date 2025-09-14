using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using WinLibraryTool.ViewModel;


namespace WinLibraryTool.Commands
{
	public class LoadCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public LoadCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members
		public void Execute(object parameter)
		{
			CommonOpenFileDialog fd = new CommonOpenFileDialog("Select the library set to load");
			fd.Filters.Add(new CommonFileDialogFilter("Windows Library Sets", "*.winLibraries;*.win7Libraries"));
			if (fd.ShowDialog() == CommonFileDialogResult.OK)
			{
				try
				{
					_viewModel.Load(fd.FileName);
				}
				catch (System.Exception ex)
				{
					WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
					{
						DialogType = WpfDialog.DialogType.Error
					};
					WpfDialog dialog = new WpfDialog(
						Helpers.AssemblyProperties.AssemblyTitle,
						String.Format("An error occurred trying to load libraries from:\n\n{0}\n\nError: {1}", fd.FileName, ex.Message),
						options
					);
				}
			}
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		#endregion
	}
}
