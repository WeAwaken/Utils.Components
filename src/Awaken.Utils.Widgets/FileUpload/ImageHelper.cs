//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Awaken.Utils.Widgets
//{
//    public static class ImageService:IImageService
//    {

//        /// <summary>
//        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
//        /// </summary>
//        /// <param name="fromFile">原图Stream对象</param>
//        /// <param name="maxWidth">最大宽(单位:px)</param>
//        /// <param name="maxHeight">最大高(单位:px)</param>
//        /// <param name="quality">图片质量：80L</param>
//        public static Stream Cut(Stream fromFile,int maxWidth, int maxHeight, long quality)
//        {
//            var resultStream = new MemoryStream();
//            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
//            Image initImage = Image.FromStream(fromFile, true);

//            ImageFormat imgFormat = initImage.RawFormat;

//            //模版的宽高比例
//            double templateRate = (double)maxWidth / maxHeight;
//            //原图片的宽高比例
//            double initRate = (double)initImage.Width / initImage.Height;

//            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
//            {
//                // 1 原图宽高均小于模版,宽高缩放按模版大小生成最终图片
//                Image tempImage = new Bitmap(maxWidth, maxHeight);
//                Graphics tempGraphics = Graphics.FromImage(tempImage);
//                tempGraphics.InterpolationMode = InterpolationMode.High;//图像质量
//                tempGraphics.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿质量
//                tempGraphics.Clear(Color.White);
//                tempGraphics.DrawImage(initImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);
                
//                tempImage.Save(resultStream, imgFormat);
//                //释放资源
//                tempGraphics.Dispose();
//                tempImage.Dispose();
//            }
//            else
//            {
//                //2 原图与模版比例相等，直接缩放

//                if (Math.Abs(templateRate - initRate) < 0.0001)
//                {
//                    // 2.1 按模版大小生成最终图片
//                    Image tempImage = new Bitmap(maxWidth, maxHeight);
//                    Graphics tempGraphics = Graphics.FromImage(tempImage);
//                    tempGraphics.InterpolationMode = InterpolationMode.High;
//                    tempGraphics.SmoothingMode = SmoothingMode.HighQuality;
//                    tempGraphics.Clear(Color.White);
//                    tempGraphics.DrawImage(initImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);
//                    tempImage.Save(resultStream, imgFormat);
//                    //释放资源
//                    tempGraphics.Dispose();
//                    tempImage.Dispose();
//                }
//                else
//                {  
//                    // 2.2 原图与模版比例不等，裁剪后缩放
//                    //裁剪对象
//                    Image pickedImage;
//                    //裁剪画布
//                    Graphics pickedGraphics;

//                    //定位
//                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
//                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    
//                    if (templateRate > initRate)
//                    {
//                        // 2.2.1 以宽为标准进行裁剪

//                        //裁剪对象实例化
//                        pickedImage = new Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
//                        pickedGraphics = Graphics.FromImage(pickedImage);

//                        //裁剪源定位
//                        fromR.X = 0;
//                        fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
//                        fromR.Width = initImage.Width;
//                        fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

//                        //裁剪目标定位
//                        toR.X = 0;
//                        toR.Y = 0;
//                        toR.Width = initImage.Width;
//                        toR.Height = (int)Math.Floor(initImage.Width / templateRate);
//                    }
//                    else
//                    { 
//                        // 2.2.2 高为标准进行裁剪
//                        pickedImage = new Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
//                        pickedGraphics = Graphics.FromImage(pickedImage);

//                        fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
//                        fromR.Y = 0;
//                        fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
//                        fromR.Height = initImage.Height;

//                        toR.X = 0;
//                        toR.Y = 0;
//                        toR.Width = (int)Math.Floor(initImage.Height * templateRate);
//                        toR.Height = initImage.Height;
//                    }

//                    //设置质量
//                    pickedGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                    pickedGraphics.SmoothingMode = SmoothingMode.HighQuality;
//                    //裁剪
//                    pickedGraphics.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);

//                    //按模版大小生成最终图片
//                    Image tempImage = new Bitmap(maxWidth, maxHeight);
//                    Graphics tempGraphics = Graphics.FromImage(tempImage);
//                    tempGraphics.InterpolationMode = InterpolationMode.High;
//                    tempGraphics.SmoothingMode = SmoothingMode.HighQuality;
//                    //tempGraphics.Clear(Color.White);
//                    tempGraphics.Clear(Color.Transparent);
//                    //根据裁剪后的img生成画布图片
//                    tempGraphics.DrawImage(pickedImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, pickedImage.Width, pickedImage.Height), GraphicsUnit.Pixel);

//                    //关键质量控制
//                    //获取系统编码类型数组,包含了jpeg>png>bmp>gif
//                    ImageCodecInfo ici = GetEncoderInfo(imgFormat) ?? GetEncoderInfo(ImageFormat.Jpeg);


//                    EncoderParameters encoderParams = new EncoderParameters(1);

//                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);//参数使用 long

//                    //保存缩放图片
//                    if (ici != null)
//                    {
//                        tempImage.Save(resultStream, ici, encoderParams);
//                    }
//                    else {
//                        tempImage.Save(resultStream, ImageFormat.Jpeg);
//                    }
                    
//                    //释放资源
//                    tempGraphics.Dispose();
//                    tempImage.Dispose();

//                    pickedGraphics.Dispose();
//                    pickedImage.Dispose();
//                }
//            }

//            //释放资源
//            initImage.Dispose();

//            // 若干该函数引发的故障：自查原因，注意stream指针位置是否在起始位置
//            resultStream.Position = 0;

//            return resultStream;
//        }
        
//        /// <summary>
//        /// 裁减图片 并添加水印
//        /// </summary>
//        /// <param name="origin"></param>
//        /// <param name="maxWidth"></param>
//        /// <param name="maxHeight"></param>
//        /// <param name="quality"></param>
//        /// <param name="content">水印内容</param>
//        /// <param name="fontName">水印字体</param>
//        /// <param name="fontSize"></param>
//        /// <param name="position"></param>
//        /// <returns></returns>
//        public static Stream Cut(Stream origin, int maxWidth, int maxHeight, long quality, string content, string fontName, int fontSize, WatermarkPostion position)
//        {
//            var resultStream = new MemoryStream();

//            // 从文件流获取原始图片，并使用流中嵌入的颜色管理信息
//            Image initImage = Image.FromStream(origin, true);

//            ImageFormat imgFormat = initImage.RawFormat;

//            //SimSun Microsoft YaHei
//            Font drawfont = new Font(fontName ?? "SimSun", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            
//            // 模版的宽高比例
//            double templateRate = (double)maxWidth / maxHeight;
            
//            // 原图片的宽高比例
//            double initRate = (double)initImage.Width / initImage.Height;

//            // 水印阴影
//            // 水印颜色
//            var fontColor=Color.FromArgb(100, 255, 255, 255);

//            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
//            {
//                #region 原图宽高均小于模版,宽高缩放按模版大小生成最终图片

//                Image tempImage = new Bitmap(maxWidth, maxHeight);

//                Graphics tempGraphics = Graphics.FromImage(tempImage);

//                tempGraphics.InterpolationMode = InterpolationMode.High;//图像质量

//                tempGraphics.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿质量

//                tempGraphics.Clear(Color.White);

//                tempGraphics.DrawImage(initImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);

//                // 加水印文字
//                if (!string.IsNullOrWhiteSpace(content))
//                {
//                    SizeF crSize = tempGraphics.MeasureString(content, drawfont);

//                    var posValue = GetPosition(tempImage.Width, tempImage.Height, crSize.Width, crSize.Height, position);

//                    float xpos = posValue.Key, ypos = posValue.Value;

//                    // 加阴影
//                    //tempGraphics.DrawString(content, drawfont, new SolidBrush(Color.FromArgb(50,0,0,0)), xpos + 1, ypos + 1);

//                    // 加文字
//                    tempGraphics.DrawString(content, drawfont, new SolidBrush(fontColor), xpos, ypos);

//                }

//                // 存储返回流
//                tempImage.Save(resultStream, imgFormat);

//                // 释放资源
//                tempGraphics.Dispose();

//                tempImage.Dispose();

//                #endregion
//            }
//            else
//            {
//                // 原图与模版比例相等，直接缩放
//                if (Math.Abs(templateRate - initRate) < 0.0001)
//                {
//                    #region 原图与模版比例相等，直接缩放

//                    //按模版大小生成最终图片
//                    Image tempImage = new Bitmap(maxWidth, maxHeight);

//                    Graphics tempGraphics = Graphics.FromImage(tempImage);

//                    tempGraphics.InterpolationMode = InterpolationMode.High;

//                    tempGraphics.SmoothingMode = SmoothingMode.HighQuality;

//                    tempGraphics.Clear(Color.White);

//                    tempGraphics.DrawImage(initImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, initImage.Width, initImage.Height), GraphicsUnit.Pixel);

//                    // 加水印文字
//                    if (!string.IsNullOrWhiteSpace(content))
//                    {
//                        SizeF crSize = tempGraphics.MeasureString(content, drawfont);

//                        var posValue = GetPosition(tempImage.Width, tempImage.Height, crSize.Width, crSize.Height,
//                            position);

//                        float xpos = posValue.Key, ypos = posValue.Value;

//                        // 加阴影
//                        //tempGraphics.DrawString(content, drawfont, new SolidBrush(Color.Black), xpos + 1, ypos + 1);

//                        // 加文字
//                        tempGraphics.DrawString(content, drawfont, new SolidBrush(fontColor), xpos, ypos);
//                    }

//                    tempImage.Save(resultStream, imgFormat);
                    
//                    //释放资源
//                    tempGraphics.Dispose();
                    
//                    tempImage.Dispose();

//                    #endregion
//                }
//                else
//                {
//                    #region 原图与模版比例不等，裁剪后缩放
//                    //裁剪对象
//                    Image pickedImage;
//                    //裁剪画布
//                    Graphics pickedGraphics;

//                    //定位
//                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
//                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

//                    //宽为标准进行裁剪
//                    if (templateRate > initRate)
//                    {
//                        //裁剪对象实例化
//                        pickedImage = new Bitmap(initImage.Width, (int)Math.Floor(initImage.Width / templateRate));
//                        pickedGraphics = Graphics.FromImage(pickedImage);

//                        //裁剪源定位
//                        fromR.X = 0;
//                        fromR.Y = (int)Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
//                        fromR.Width = initImage.Width;
//                        fromR.Height = (int)Math.Floor(initImage.Width / templateRate);

//                        //裁剪目标定位
//                        toR.X = 0;
//                        toR.Y = 0;
//                        toR.Width = initImage.Width;
//                        toR.Height = (int)Math.Floor(initImage.Width / templateRate);
//                    }
//                    //高为标准进行裁剪
//                    else
//                    {
//                        pickedImage = new Bitmap((int)Math.Floor(initImage.Height * templateRate), initImage.Height);
//                        pickedGraphics = Graphics.FromImage(pickedImage);

//                        fromR.X = (int)Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
//                        fromR.Y = 0;
//                        fromR.Width = (int)Math.Floor(initImage.Height * templateRate);
//                        fromR.Height = initImage.Height;

//                        toR.X = 0;
//                        toR.Y = 0;
//                        toR.Width = (int)Math.Floor(initImage.Height * templateRate);
//                        toR.Height = initImage.Height;
//                    }

//                    //设置质量
//                    pickedGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                    pickedGraphics.SmoothingMode = SmoothingMode.HighQuality;
//                    //裁剪
//                    pickedGraphics.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);

//                    //按模版大小生成最终图片
//                    Image tempImage = new Bitmap(maxWidth, maxHeight);
//                    Graphics tempGraphics = Graphics.FromImage(tempImage);
//                    tempGraphics.InterpolationMode = InterpolationMode.High;
//                    tempGraphics.SmoothingMode = SmoothingMode.HighQuality;
//                    //tempGraphics.Clear(Color.White);
//                    tempGraphics.Clear(Color.Transparent);
//                    //根据裁剪后的img生成画布图片
//                    tempGraphics.DrawImage(pickedImage, new Rectangle(0, 0, maxWidth, maxHeight), new Rectangle(0, 0, pickedImage.Width, pickedImage.Height), GraphicsUnit.Pixel);
                    
//                    // 加水印文字
//                    if (!string.IsNullOrWhiteSpace(content))
//                    {
//                        SizeF crSize = tempGraphics.MeasureString(content, drawfont);

//                        var posValue = GetPosition(tempImage.Width, tempImage.Height, crSize.Width, crSize.Height,
//                            position);

//                        float xpos = posValue.Key, ypos = posValue.Value;

//                        // 加阴影
//                        //tempGraphics.DrawString(content, drawfont, new SolidBrush(Color.Black), xpos + 1, ypos + 1);

//                        // 加文字
//                        tempGraphics.DrawString(content, drawfont, new SolidBrush(fontColor), xpos, ypos);
//                    }

//                    //关键质量控制
//                    //获取系统编码类型数组,包含了jpeg>png>bmp>gif
//                    ImageCodecInfo ici = GetEncoderInfo(imgFormat) ??GetEncoderInfo(ImageFormat.Jpeg);

//                    EncoderParameters encoderParams = new EncoderParameters(1);

//                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);//参数使用 long

//                    //保存缩放图片
//                    if (ici != null)
//                    {
//                        tempImage.Save(resultStream, ici, encoderParams);
//                    }
//                    else {
//                        tempImage.Save(resultStream, ImageFormat.Jpeg);
//                    }                    

//                    //释放资源
//                    tempGraphics.Dispose();
//                    tempImage.Dispose();

//                    pickedGraphics.Dispose();
//                    pickedImage.Dispose();

//                    #endregion
//                }
//            }

//            //释放资源
//            initImage.Dispose();

//            resultStream.Position = 0;

//            return resultStream;

//        }


//        /// <summary>
//        /// 广告词水印处理
//        /// </summary>
//        /// <param name="origin"></param>
//        /// <param name="content"></param>
//        /// <param name="position"></param>
//        /// <param name="fontName"></param>
//        /// <param name="fontSize"></param>
//        /// <returns></returns>
//        public static Stream Mark(Stream origin, string content, MarkPostion position, string fontName, int fontSize)
//        {
//            Image img = Image.FromStream(origin, true);

//            ImageFormat imgFormat = img.RawFormat;

//            Graphics g = Graphics.FromImage(img);

//            //SimSun Microsoft YaHei
//            Font drawfont = new Font(fontName ?? "SimSun", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

//            SizeF crSize = g.MeasureString(content, drawfont);

//            var posValue = GetPosition(img.Width, img.Height, crSize.Width, crSize.Height, position);

//            float xpos = posValue.Key, ypos = posValue.Value;

//            g.DrawString(content, drawfont, new SolidBrush(Color.Black), xpos, ypos);

//            var resultStream = new MemoryStream();

//            img.Save(resultStream, imgFormat);

//            g.Dispose();

//            img.Dispose();

//            resultStream.Position = 0;

//            return resultStream;
//        }

//        /// <summary>
//        /// 将Base64代码转换成图片流
//        /// </summary>
//        /// <param name="code"></param>
//        /// <returns></returns>
//        public static Stream Base64ToImage(string code)
//        {
//            byte[] bytes = Convert.FromBase64String(code);

//            MemoryStream resultStream = new MemoryStream(bytes);

//            //Bitmap bmp = new Bitmap(ms);
//            //bmp.Save("1.png", System.Drawing.Imaging.ImageFormat.Png);

//            resultStream.Position = 0;

//            return resultStream;
//        }


//        #region private methods

//        /// <summary>
//        /// 位置确定
//        /// </summary>
//        /// <param name="position"></param>
//        /// <returns></returns>
//        private static KeyValuePair<float, float> GetPosition(float imgWidth,float imgHeight,float fontWidth,float fontHeight ,MarkPostion position)
//        {
//            float xpos = 0, ypos = 0;

//            switch (position)
//            {
//                case MarkPostion.LeftTop:
//                    xpos = imgWidth*.01f;
//                    ypos = imgWidth*.01f;
//                    break;
//                case MarkPostion.LeftBottom:
//                    xpos = imgWidth*.01f;
//                    ypos = imgHeight*.99f - fontHeight;
//                    break;
//                case MarkPostion.RightTop:
//                    xpos = imgWidth*.99f - fontWidth;
//                    ypos = imgHeight*.01f;
//                    break;
//                case MarkPostion.RightBottom:
//                    xpos = imgWidth*.99f - fontWidth;
//                    ypos = imgHeight*.99f - fontHeight;
//                    break;
//            }

//            return new KeyValuePair<float, float>(xpos, ypos);
//        }
        
//        private static ImageCodecInfo GetEncoderInfo(ImageFormat imageFormat)
//        {
//            string mimeType = "image/jpeg";

//            if (imageFormat.Equals(ImageFormat.Jpeg))
//            {
//                mimeType = "image/jpeg";
//            }
//            else if (imageFormat.Equals(ImageFormat.Png))
//            {
//                mimeType = "image/png";
//            }
//            else if (imageFormat.Equals(ImageFormat.Gif))
//            {
//                mimeType = "image/gif";
//            }
//            else if (imageFormat.Equals(ImageFormat.Bmp))
//            {
//                mimeType = "image/bmp";
//            }

//            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

//            return encoders.FirstOrDefault(t => t.MimeType == mimeType);
//        }

//        /// <summary>
//        /// 判断文件类型是否为WEB格式图片
//        /// (注：JPG,GIF,BMP,PNG)
//        /// </summary>
//        /// <param name="contentType">HttpPostedFile.ContentType</param>
//        /// <returns></returns>
//        public static bool IsWebImage(string contentType)
//        {
//            if (contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
//            {
//                return true;
//            }
//            return false;
//        }

//        #endregion
//    }
//}
