using System;
using System.Windows.Input;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class RemoveFolderCommand : ICommand
	{
		readonly LibraryViewModel _viewModel;

		public RemoveFolderCommand(LibraryViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			_viewModel.RemoveFolder(_viewModel.CurrentFolder.FolderName);
		}

		public bool CanExecute(object parameter)
		{
			return (_viewModel.CanSetSave());
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
