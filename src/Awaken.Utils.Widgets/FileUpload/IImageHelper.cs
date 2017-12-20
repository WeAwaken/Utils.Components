using System.IO;

namespace Awaken.Utils.Widgets
{
    public interface IImageHelper
    {

        /// <summary>
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">图片质量：80L</param>
        Stream Cut(Stream fromFile, int maxWidth, int maxHeight, long quality);


        /// <summary>
        /// 裁减图片[添加水印]
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="quality"></param>
        /// <param name="content">水印内容</param>
        /// <param name="fontName">水印字体</param>
        /// <param name="fontSize"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Stream Cut(Stream origin, int maxWidth, int maxHeight, long quality, string content, string fontName, int fontSize, WatermarkPostion position);


        /// <summary>
        /// 广告词水印处理
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="content"></param>
        /// <param name="position"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        Stream WaterMark(Stream origin, string content, WatermarkPostion position, string fontName, int fontSize);

        /// <summary>
        /// 将Base64代码转换成图片流
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Stream Base64ToImage(string code);

        #region private methods
        
        /// <summary>
        /// 判断文件类型是否为WEB格式图片
        /// (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        bool IsWebImage(string contentType);
        

        #endregion
    }

    /// <summary>
    /// 水印位置
    /// </summary>
    public enum WatermarkPostion
    {
        /// <summary>
        /// 左上角
        /// </summary>
        LeftTop,
        /// <summary>
        /// 左下角
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 右上角
        /// </summary>
        RightTop,
        /// <summary>
        /// 右下角
        /// </summary>
        RightBottom,
    }
}
