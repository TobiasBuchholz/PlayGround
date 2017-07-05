using Foundation;

namespace System
{
	public static class StringExtensions
	{
		public static NSAttributedString ToAttributedHtmlText(this string htmlText)
		{
			NSError error = null;
			return new NSAttributedString(htmlText, new NSAttributedStringDocumentAttributes { DocumentType = NSDocumentType.HTML }, ref error);
		}
	}
}
