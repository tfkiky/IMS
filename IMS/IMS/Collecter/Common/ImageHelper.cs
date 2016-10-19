using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;

namespace IMS.Collecter.Common
{
    class ImageHelper
    {
        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }
        /// <summary>
        /// 图片保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buffer"></param>
        public static void ImageSave(string fileName, byte[] buffer)
        {
            Image image = BytesToImage(buffer);
            image.Save(fileName);
        }

    }
}
