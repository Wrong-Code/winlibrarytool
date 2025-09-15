﻿using BusinessLib.DataModel;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WinLibraryTool.Helpers.CodeHelpers.IconHelper;
using WinLibraryTool.ViewModel;

namespace WinLibraryTool.UserControls
{
	/// <summary>
	/// Interaction logic for LibraryIconSelectorControl.xaml
	/// </summary>
	public partial class LibraryIconSelectorControl : UserControl
	{
		// Resource IDs for known library icons from imageres.dll:
		// Documents:	-1002
		// Pictures:	-1003
		// Music:		-1004
		// Videos:		-1005

		private const string _defaultIconFile = "imageres.dll";
		private readonly List<int> _defaultIconIndexes = new List<int> { -1002, -1003, -1004, -1005 };

		private readonly LibraryViewModel _viewModel;
		private IconReference _currentIconReference;
		private readonly ICommand _browseCommand;
		private ObservableCollection<AvailableIcon> _availableIcons;
		private Window _userInterface;
		private readonly string _defaultIconPath;

		public LibraryIconSelectorControl()
			:this(null, null)
		{
		}

		public LibraryIconSelectorControl(LibraryViewModel viewModel, Window userInterface)
		{
			InitializeComponent();

			_userInterface = userInterface;
			_viewModel = viewModel;
			_currentIconReference = _viewModel.IconReference;
			_browseCommand = new Commands.BrowseCommand(this);
			_availableIcons = new ObservableCollection<AvailableIcon>();
			_defaultIconPath = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32\\imageres.dll");

			Run linkText = new Run(_defaultIconPath);
			Hyperlink link = new Hyperlink(linkText);
			//link.NavigateUri = new Uri(webAddress.Text);
			link.Click +=new RoutedEventHandler(link_Click);
			tipText.Inlines.Add("Tip: Try browsing ");
			tipText.Inlines.Add(link);

			AddDefaultIcons();

			// add the icon(s) from the referenced file, unless it is one of the defaults.
			if (!IsBuiltInLibraryIcon(_currentIconReference))
			{
				AddIcons(_currentIconReference.ModuleName);
			}

			base.DataContext = this;
		}

		void link_Click(object sender, RoutedEventArgs e)
		{
			AddIcons(_defaultIconPath);
		}

		private bool IsBuiltInLibraryIcon(IconReference iconRef)
		{
			bool isBuiltIn = false;

			if ((System.IO.Path.GetFileName(iconRef.ModuleName).Equals(_defaultIconFile, StringComparison.InvariantCultureIgnoreCase) &&
				_defaultIconIndexes.Contains(iconRef.ResourceId)) || 
				iconRef.ReferencePath.Equals(WinLibrary.DefaultIconReference, StringComparison.InvariantCultureIgnoreCase))
			{
				isBuiltIn = true;
			}

			return isBuiltIn;
		}

		public ICommand BrowseCommand
		{
			get { return _browseCommand; }
		}

		private void AddDefaultIcons()
		{
			var systemFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);

			foreach (int defaultIconIndex in _defaultIconIndexes)
			{
				IconReference iconRef = new IconReference(System.IO.Path.Combine(systemFolder, _defaultIconFile), defaultIconIndex);
				_availableIcons.Add(new AvailableIcon(iconRef));
			}
		}

		public void AddIcons(string filePath)
		{
			int iconCount = IconHelper.GetIconCount(filePath);
			if (iconCount == 0)
			{
				WpfDialog.WpfDialogOptions errorOptions = new WpfDialog.WpfDialogOptions
				{
					DialogType = WpfDialog.DialogType.Error,
					TitleBarIcon = _userInterface.Icon
				};

				WpfDialog dialog = new WpfDialog(
					Helpers.AssemblyProperties.AssemblyTitle,
					String.Format("No icons were found in '{0}'.", System.IO.Path.GetFileName(filePath)),
					errorOptions
				) {
					Owner = _userInterface
				};
				dialog.ShowDialog();
			}
			else
			{
				Mouse.OverrideCursor = Cursors.Wait;
				int errorCount = 0;

				for (int index = 0; index < iconCount; index++)
				{
					IconReference iconRef = new IconReference(filePath, index);
					AvailableIcon icon = null;

					try
					{
						icon = new AvailableIcon(iconRef);
					}
					catch (Exception)
					{
						errorCount++;
					}

					if (icon != null)
					{
						_availableIcons.Add(icon);
					}
				}

				Mouse.OverrideCursor = null;
				if (errorCount > 0)
				{
					WpfDialog.WpfDialogOptions options = new WpfDialog.WpfDialogOptions
					{
						DialogType = WpfDialog.DialogType.Warning,
						TitleBarIcon = _userInterface.Icon
					};
					WpfDialog dialog = new WpfDialog(
						Helpers.AssemblyProperties.AssemblyTitle,
						String.Format("{0} icon(s) were not added from '{1}', possibly because no 32x32 size was found.", errorCount, System.IO.Path.GetFileName(filePath)),
						options
					) {
						Owner = _userInterface
					};
					dialog.ShowDialog();
				}
			}
		}

		public ObservableCollection<AvailableIcon> AvailableIcons
		{
			get { return _availableIcons; }
		}

		public IconReference CurrentIconReference
		{
			get { return _currentIconReference; }
		}

		private void iconList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count >= 1)
			{
				_currentIconReference = ((AvailableIcon)e.AddedItems[0]).IconReference;
			}
		}
	}

	public class AvailableIcon
	{
		private IconReference _iconReference;
		private ImageSource _iconImage;

		public AvailableIcon(IconReference iconReference)
		{
			_iconReference = iconReference;
			_iconImage = IconHelper.IconReferenceToImageSource(_iconReference);
		}

		public IconReference IconReference
		{
			get { return _iconReference; }
		}

		public ImageSource IconImage
		{
			get { return _iconImage; }
		}
	}
}
