using System;
using System.Windows.Input;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class LoadExistingCommand : ICommand
	{
		readonly LibrarySetViewModel _viewModel;

		public LoadExistingCommand(LibrarySetViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			_viewModel.LoadExisting();
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
