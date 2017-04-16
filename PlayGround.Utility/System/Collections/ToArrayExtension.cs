namespace System.Collections
{
	public static class ToArrayExtension
	{
		public static T[] ToSingleArray<T>(this T @this)
		{
			var array = new T[1];
			array[0] = @this;
			return array;
		}
	}
}
