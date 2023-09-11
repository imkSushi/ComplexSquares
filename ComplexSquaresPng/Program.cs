using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;

namespace ComplexSquaresPng
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            
            if (!int.TryParse(input, out var width)) 
                Fail($"Expected integer input, found: '{input}'");

            if (width < 1)
                Fail($"Expected input to be a positive integer, found: '{width}'");

            var b = new Bitmap(width, width);

            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
                b.SetPixel(i, j, Color.White);
            
            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
            {
                var x = i * i - j * j;
                x %= width;
                if (x < 0) 
                    x += width;
                var y = (2 * i * j) % width;
                b.SetPixel(x, y, Color.Black);
            }
            
            b.Save($"Complex Squares {width}.png", ImageFormat.Png);

            var b2 = new int[width, width];
            
            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
            {
                b2[i, j] = 0;
            }

            const int blur = 2;
            const int blurSquare = blur * blur * 4 + blur * 4 + 1;
            const int blurSquare255 = blurSquare * 255;
            
            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
            {
                if (b.GetPixel(i, j).R == 255)
                    continue;
                for (var k = -blur; k <= blur; k++)
                for (var m = -blur; m <= blur; m++)
                {
                    var x = i + k;
                    var y = j + m;
                    if (x < 0)
                        x += width;
                    else if (x >= width)
                        x -= width;
                    if (y < 0)
                        y += width;
                    else if (y >= width)
                        y -= width;
                    b2[x, y] += 255;
                }
            }

            for (var i = 0; i < width; i++)
            for (var j = 0; j < width; j++)
            {
                var sum = (blurSquare255 - b2[i, j]) / blurSquare;
                b.SetPixel(i, j, Color.FromArgb(sum, sum, sum));
            }
            
            b.Save($"Complex Squares {blur} Blurred {width}.png", ImageFormat.Png);
        }

        [DoesNotReturn]
        private static void Fail(string reason)
        {
            Console.Out.WriteLine(reason);
            Environment.Exit(1);
        } 
    }
}