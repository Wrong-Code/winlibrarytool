using Microsoft.WindowsAPICodePack.Shell;
using System;
using WinLibraryTool.Helpers.CodeHelpers.IconHelper;

namespace WinLibraryTool.Converters
{
	public class IconReferenceToBitmap : ConverterMarkupExtension<IconReferenceToBitmap>
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null && value is IconReference)
			{
				return IconHelper.IconReferenceToImageSource((IconReference)value);
			}

			return value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
