using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinLibraryTool.Helpers
{
	/// <summary>
	/// Provides static access to system dialog icons (Info, Warning, Error, Question).
	/// Icons are loaded once and cached to avoid issues with COM interop and resource exhaustion.
	/// </summary>
	public static class SystemIcons
	{
		public static ImageSource Info { get; private set; }
		public static ImageSource Warning { get; private set; }
		public static ImageSource Error { get; private set; }
		public static ImageSource Question { get; private set; }

		/// <summary>
		/// Static constructor to load system icons once at first access.
		/// </summary>
		static SystemIcons()
		{
			try
			{
				var icons = new StockIcons();

				Info     = SafeClone(icons.Info?.BitmapSource);
				Warning  = SafeClone(icons.Warning?.BitmapSource);
				Error    = SafeClone(icons.Error?.BitmapSource);
				Question = SafeClone(icons.Help?.BitmapSource);
			}
			catch
			{
				// Assign nulls if any issue occurs; optionally set fallback icons here
				Info = Warning = Error = Question = null;
			}
		}

		/// <summary>
		/// Clones a BitmapSource into a new WriteableBitmap to avoid deferred rendering issues.
		/// </summary>
		private static ImageSource SafeClone(BitmapSource source)
		{
			return source != null ? new WriteableBitmap(source) : null;
		}

		/// <summary>
		/// Optionally call at app startup to eagerly load icons and avoid runtime surprises.
		/// </summary>
		public static void WarmUp()
		{
			_ = Info;
			_ = Warning;
			_ = Error;
			_ = Question;
		}
	}
}
