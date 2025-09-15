using System;
using System.Windows.Input;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class SetSaveCommand : ICommand
	{
		readonly LibraryViewModel _viewModel;

		public SetSaveCommand(LibraryViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			_viewModel.SetSaveFolder(_viewModel.CurrentFolder.FolderName);
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
