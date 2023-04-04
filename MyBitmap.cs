using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Shapes;
using Rectangle = System.Drawing.Rectangle;
using System.Security.RightsManagement;
using System.Windows.Markup;
using System.Reflection;

namespace ProjektASM
{
    internal static class MyBitmap
    {
        [DllImport(@"D:\Projekty\Grayscale\ProjektASM\x64\Debug\JAAsm.dll", CallingConvention = CallingConvention.Cdecl)]
        extern static int MyProc1(byte[] bitmap,int index);

        public static Bitmap ColorBalance(this Bitmap sourceBitmap, uint threadsNumber, bool csLanguage)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];


            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);


            sourceBitmap.UnlockBits(sourceData);

            Parallel.For(0, pixelBuffer.Length, new ParallelOptions { MaxDegreeOfParallelism = (int)threadsNumber }, k =>
            {
                if (k * 4 < pixelBuffer.Length)
                {
                    if (csLanguage)
                    {

                        int i = k * 4;
                        float blue = (float)pixelBuffer[i];
                        float green = (float)pixelBuffer[i + 1];
                        float red = (float)pixelBuffer[i + 2];

                        float grayscale = (float)((blue + green + red) / 3);

                        pixelBuffer[i] = (byte)grayscale;
                        pixelBuffer[i + 1] = (byte)grayscale;
                        pixelBuffer[i + 2] = (byte)grayscale;
                    }

                    else if (csLanguage == false)
                    {

                        int i = k * 4;

                        int grayscale = MyProc1(pixelBuffer,i);

                        pixelBuffer[i] = (byte)grayscale;
                        pixelBuffer[i + 1] = (byte)grayscale;
                        pixelBuffer[i + 2] = (byte)grayscale;
                    }
                }
            });


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                       ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }
    }
}
