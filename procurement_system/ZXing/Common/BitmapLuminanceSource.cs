using System.IO;

namespace ZXing.Common
{
    internal class BitmapLuminanceSource
    {
        private MemoryStream stream;

        public BitmapLuminanceSource(MemoryStream stream)
        {
            this.stream = stream;
        }
    }
}