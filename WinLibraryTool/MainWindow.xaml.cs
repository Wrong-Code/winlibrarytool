using System;
using System.Windows;
using Dark.Net;
using BusinessLib.DataModel;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool
{
	/// <summary>
  /// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DarkNet.Instance.SetWindowThemeWpf(this, App.Theme);
      
      // Get raw library data.
      WinLibrary[] libraries = new WinLibrary[] { };

			// Create UI-friendly wrappers around the 
			// raw data objects (i.e. the view-model).
			LibrarySetViewModel _viewModel = new LibrarySetViewModel(libraries, this);

			// Let the UI bind to the view-model.
			base.DataContext = _viewModel;
		}

		private void btnExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void libraryTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			LibraryViewModel selectedLibrary = libraryTree.SelectedItem as LibraryViewModel;
			if (selectedLibrary == null)
			{
				FolderViewModel selectedFolder = libraryTree.SelectedItem as FolderViewModel;
				if (selectedFolder != null)
				{
					selectedLibrary = selectedFolder.Parent as LibraryViewModel;
				}
			}

			if (selectedLibrary != null)
			{
				((LibrarySetViewModel)DataContext).CurrentLibrary = selectedLibrary;
			}
		}

		private void btnDelete_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnDelete.Opacity = btnDelete.IsEnabled ? 1.0 : 0.5;
		}

		private void btnEdit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnEdit.Opacity = btnEdit.IsEnabled ? 1.0 : 0.5;
		}

		private void btnSave_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			btnSave.Opacity = btnSave.IsEnabled ? 1.0 : 0.5;
		}

    protected override void OnContentRendered(EventArgs e)
    {
      base.OnContentRendered(e);

      var vm = DataContext as LibrarySetViewModel;

      if (vm?.LoadExistingCommand?.CanExecute(null) == true)
      {
        vm.LoadExistingCommand.Execute(null);
      }
    }
  }
}
