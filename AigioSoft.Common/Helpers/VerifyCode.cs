#if NET20 || NET35 || NET40 || NET47

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace AigioSoft.Common.Helpers
{
    public class VerifyCode
    {
        public string CreateImageToBase64(Random random, out string code)
        {
            code = GetRandomCode(random);
            var img = CreateImageCode(code);
            return ImgToBase64String(img);
        }

        public Bitmap CreateImage(Random random, out string code)
        {
            code = GetRandomCode(random);
            return CreateImageCode(code);
        }

        public string GetRandomCode(Random random)
        {
            string randString = string.Empty;
            do
            {
                randString += RandCharString.Substring(random.Next(DateTime.Now.Millisecond) % RandCharString.Length, 1);
            }
            while (randString.Length < Length);
            return randString;
        }

        private static string ImgToBase64String(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
            return "data:image/jpg;base64," + Convert.ToBase64String(arr);
        }

        #region 配置参数

        /// <summary>
        /// 验证码随机字符组
        /// </summary>
        public string RandCharString { get; set; } = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        /// <summary>
        /// 验证码长度
        /// </summary>
        public int Length { get; set; } = 4;

        /// <summary>
        /// 验证码字体大小
        /// </summary>
        public int FontSize { get; set; } = 14;

        /// <summary>
        /// 字符内边距
        /// </summary>
        public int Padding { get; set; } = 6;

        /// <summary>
        /// 是否输出燥点
        /// </summary>
        public bool Chaos { get; set; } = true;

        /// <summary>
        /// 输出燥点的颜色
        /// </summary>
        public Color ChaosColor { get; set; } = Color.LightGray;

        /// <summary>
        /// 自定义背景色
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors { get; set; } = { Color.FromArgb(0, 153, 204), Color.FromArgb(51, 153, 102), Color.FromArgb(102, 51, 153), Color.FromArgb(255, 204, 102), Color.FromArgb(255, 51, 51), Color.FromArgb(153, 204, 255), Color.FromArgb(51, 102, 153), Color.FromArgb(0, 102, 102) };

        /// <summary>
        /// 自定义字体数组
        /// </summary>
        public string[] Fonts { get; set; } = { "Arial", "Georgia", "Microsoft Sans Serif", "Segoe UI Semilight", "Microsoft YaHei UI", "Buxton Sketch", "Courier", "Constantia", "Maiandra GD" };

        public int ImageWidth { get; set; } = 272;
        public int ImageHeight { get; set; } = 120;

        #endregion 配置参数

        #region 产生波形滤镜效果

        private const double Pi2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    var dx = bXDir ? Pi2 * j / dBaseAxisLen : Pi2 * i / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    // 取得当前点的颜色
                    var nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    var nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }

        #endregion 产生波形滤镜效果

        #region 生成校验码图片

        private Bitmap CreateImageCode(string code)
        {
            var image = new Bitmap(ImageWidth, ImageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            Random rand = new Random(DateTime.Now.Ticks.GetHashCode());
            if (Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;
                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }
            int n1 = ImageHeight - FontSize - Padding * 2;
            int n2 = n1 / 4;
            var top1 = n2;
            var top2 = n2 * 2;
            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                var cindex = rand.Next(Colors.Length - 1);
                var findex = rand.Next(Fonts.Length - 1);
                var f = new Font(Fonts[findex], FontSize, FontStyle.Bold);
                Brush b = new SolidBrush(Colors[cindex]);
                var top = i % 2 == 1 ? top2 : top1;
                var left = i * (FontSize + Padding);

                Matrix matrix = new Matrix();
                matrix.RotateAt(rand.Next(-2, 3), new PointF(top + ImageWidth / 2, left - FontSize / 2));
                g.Transform = matrix;
                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }
            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();
            //产生波形
            //image = TwistImage(image, false, 3, 4);
            return image;
        }

        #endregion 生成校验码图片
    }
}
#endif