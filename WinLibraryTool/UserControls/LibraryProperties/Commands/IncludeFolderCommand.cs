using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Input;
using System.Windows;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.Commands
{
	public class IncludeFolderCommand : ICommand
	{
		readonly LibraryViewModel _viewModel;

		public IncludeFolderCommand(LibraryViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#region ICommand Members

		public void Execute(object parameter)
		{
			CommonOpenFileDialog fd = new CommonOpenFileDialog("Select folder(s) to add");
			fd.IsFolderPicker = true; 
			fd.Multiselect = true;

			bool cancelled = false;
			bool succeeded = false;
			do
			{
				if (fd.ShowDialog() == CommonFileDialogResult.OK)
				{
					foreach (var folderPath in fd.FileNames)
					{
						if (! _viewModel.IncludeFolder(folderPath))
						{
							WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
							{
								DialogType = WpfDialog.DialogType.Error,
								TitleBarIcon = ((Window) parameter).Icon
							};
							WpfDialog dialog = new WpfDialog(
								Helpers.AssemblyProperties.AssemblyTitle,
								String.Format("The folder '{0}' already exists in this library.\n\nPlease choose another.", folderPath),
								options
							) {
								Owner = (Window) parameter
							};
							dialog.ShowDialog();
							return;
						}
					}
					succeeded = true;
				}
				else
				{
					cancelled = true;
				}
			} while (!cancelled && !succeeded);

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
