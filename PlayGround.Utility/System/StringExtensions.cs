using System.Text.RegularExpressions;

namespace System
{
	public static class StringExtensions
	{
		public static string StripHTML(this string input)
		{
			return Regex.Replace(input, "<.*?>", string.Empty);
		}
	}
}
