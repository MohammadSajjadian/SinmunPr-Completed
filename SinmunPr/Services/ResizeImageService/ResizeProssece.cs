using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Services
{
    public class ResizeProssece : IResize
    {
        public byte[] Resizer(byte[] b, int width, int height, ImageFormat format)
        {
            MemoryStream memoryStream = new MemoryStream(b);
            Image image = Image.FromStream(memoryStream);
            Bitmap bitmap = new Bitmap(image, width, height);

            MemoryStream memoryStream1 = new MemoryStream();
            bitmap.Save(memoryStream1, format);

            return memoryStream1.ToArray();
        }
    }
}
