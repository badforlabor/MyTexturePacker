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
        static void SmartProcess(string inPath, string inPattern, string output)
        {
            /************************************************************************/
            /* 更智能的处理：
             *       以bottom位置为中心，对准所有图片
             *       自动填充到128*128,256*256,512*512...
            /************************************************************************/

            string[] files = Directory.GetFiles(inPath, inPattern + "-*.png");

            // 排序
            {
                List<string> filelist = new List<string>(files);
                filelist.Sort(delegate (string left, string right)
                {
                    left = left.Substring(left.LastIndexOf("-") + 1);
                    right = right.Substring(right.LastIndexOf("-") + 1);
                    int lefta = int.Parse(left.Substring(0, left.IndexOf(".png")));
                    int righta = int.Parse(right.Substring(0, right.IndexOf(".png")));

                    return lefta - righta;
                });
                files = filelist.ToArray();
            }

            if (files.Length > 0)
            {
                Console.WriteLine("files.length=" + files.Length);

                // 创建一个512*512的图片
                int width = 512, height = 512;
                Image img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);

                int MaxWidth = 0;
                int MaxHeight = 0;

                foreach (var file in files)
                {
                    using (Image sourceImg = Image.FromFile(file))
                    {
                        if (sourceImg == null)
                            continue;

                        MaxWidth = Math.Max(sourceImg.Width, MaxWidth);
                        MaxHeight = Math.Max(sourceImg.Height, MaxHeight);
                    }
                }

                Point p = Point.Empty;
                int cnt = 0;
                foreach (var file in files)
                {
                    Image sourceImg = Image.FromFile(file);
                    
                    if (sourceImg == null)
                        continue;
                                       

                    if (p.X + MaxWidth > width)
                    {
                        p.X = 0;
                        p.Y += MaxHeight;
                    }

                    // 如果越界了，那么终止操作
                    if (p.X + MaxWidth > width || p.Y + MaxHeight > height)
                        break;

                    // 以底部对齐方式绘图
                    Point tp = p;
#if bottom
                    tp.X += (MaxWidth - sourceImg.Width) / 2;
                    tp.Y += (MaxHeight - sourceImg.Height);
#elif center
                    tp.X += (MaxWidth - sourceImg.Width) / 2;
                    tp.Y += (MaxHeight - sourceImg.Height) / 2;

#else // lefttop
//                     tp.X += (MaxWidth - sourceImg.Width) / 2;
//                     tp.Y += (MaxHeight - sourceImg.Height) / 2;
#endif
                    g.DrawImage(sourceImg, tp);
                    cnt++;

                    p.X += MaxWidth;
                }
                Directory.CreateDirectory(output);
                img.Save(output + "\\" + inPattern + "_size" + MaxWidth + "x" + MaxHeight + "x" + cnt + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        static void Main(string[] args)
        {
            Console.ReadKey();
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
                SmartProcess(inPath, "" + (inPattern + i), output);
            }
        }
#endif
                }
}
