using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Services
{
    public interface IResize
    {
        public byte[] Resizer(byte[] b, int width, int height, ImageFormat format);
    }
}
