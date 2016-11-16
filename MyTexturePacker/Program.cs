using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace MyTexturePacker
{
    class Program
    {
#if old
        static void Main(string[] args)
        {
            // 读取目录里面的文件：
            string inPath = @"D:\\liubo\bitbucket\\临时美术\\狐美人\\";
            string inPattern = "7769-*.png";
            string output = @"D:\\liubo\bitbucket\\临时美术\\狐美人\\humeiren_01.png";

            if (args.Length != 3)
            {
                Console.WriteLine("args[0]=" + args[0]);
                return;
            }
            inPath = args[0];
            inPattern = args[1];
            output = args[2];

            string[] files = Directory.GetFiles(inPath, inPattern);
            if (files.Length > 0)
            {
                Console.WriteLine("files.length=" + files.Length);

                // 创建一个512*512的图片
                int width = 512, height = 512;
                Image img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);

                Point p = Point.Empty;
                foreach (var file in files)
                {
                    Image sourceImg = Image.FromFile(file);

                    if (sourceImg == null)
                        continue;

                    if (p.X + sourceImg.Width > width)
                    {
                        p.X = 0;
                        p.Y += sourceImg.Height;
                    }

                    // 如果越界了，那么终止操作
                    if (p.X + sourceImg.Width > width || p.Y + sourceImg.Height > height)
                        break;

                    g.DrawImage(sourceImg, p);
                    p.X += sourceImg.Width;
                }
                Directory.CreateDirectory(output.Substring(0, output.LastIndexOf("\\")));
                img.Save(output, System.Drawing.Imaging.ImageFormat.Png);
            }            
        }
#else
        static void Process(string inPath, string inPattern, string output)
        {
            string[] files = Directory.GetFiles(inPath, inPattern + "-*.png");
            if (files.Length > 0)
            {
                Console.WriteLine("files.length=" + files.Length);

                // 创建一个512*512的图片
                int width = 512, height = 512;
                Image img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);

                Point p = Point.Empty;
                foreach (var file in files)
                {
                    Image sourceImg = Image.FromFile(file);

                    if (sourceImg == null)
                        continue;

                    if (p.X + sourceImg.Width > width)
                    {
                        p.X = 0;
                        p.Y += sourceImg.Height;
                    }

                    // 如果越界了，那么终止操作
                    if (p.X + sourceImg.Width > width || p.Y + sourceImg.Height > height)
                        break;

                    g.DrawImage(sourceImg, p);
                    p.X += sourceImg.Width;
                }
                Directory.CreateDirectory(output);
                img.Save(output + "\\" + inPattern + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        static void Main(string[] args)
        {
            // 读取目录里面的文件：
            string inPath = @"D:\\liubo\bitbucket\\临时美术\\狐美人\\";
            int inPattern = 7769;
            string output = @"D:\\liubo\bitbucket\\临时美术\\狐美人\\output\\";
            int inCount = 1;
            
            if (args.Length != 4)
            {
                Console.WriteLine("args[0]=" + args[0]);
                foreach(var arg in args)
                    Console.WriteLine("arg=" + arg);
                return;
            }
            inPath = args[0];
            inPattern = int.Parse(args[1]);
            output = args[2];
            inCount = int.Parse(args[3]);

            for (int i = 0; i < inCount; i++)
            {
                Process(inPath, "" + (inPattern + i), output);
            }
        }
#endif
    }
}
