using System.IO;

namespace QuickLookLibrary
{
	public enum Icons
	{
		Xamagon,
		None
	}
	
	public class ImageProvider
	{
		public ImageProvider ()
		{
		}

		public Icons GetIconType (string path)
		{
			// This code could also return paths
			if (Path.GetExtension (path) == ".xam")
			{
				return Icons.Xamagon;
			}
			return Icons.None;
		}
	}
}
