using Qiniu.Http;
using System.IO;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 文件上传处理接口
    /// </summary>
    public interface IFileUpload
    {
        /// <summary>
        /// 上传[覆盖模式]
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fname"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        string Upload(Stream fileStream, string fname, string bucketName);

        /// <summary>
        /// 异步上传[覆盖模式]
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fname"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<string> UploadAsync(Stream fileStream, string fname, string bucketName);

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        void Delete(string fileName, string bucketName);

        /// <summary>
        /// 异步删除单个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task DeleteAsync(string fileName, string bucketName);

        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        void BatchDelete(string[] fileNames, string bucketName);

        /// <summary>
        /// 异步批量删除文件
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task BatchDeleteAsync(string[] fileNames, string bucketName);

        /// <summary>
        /// 生成授权的下载链接(访问私有空间中的文件时需要使用)
        /// </summary>
        /// <param name="rawUrl">初始链接</param>
        /// <param name="expireInSeconds">下载有效时间(单位:秒)</param>
        /// <returns>包含过期时间的已授权的下载链接</returns>
        string CreateSignedUrl(string rawUrl, int expireInSeconds = 3600);

        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        void Download(string rawUrl, string saveFilePath);

        /// <summary>
        /// 私有文件下载
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="expireInSeconds"></param>
        /// <returns></returns>
        void DownloadPrivate(string rawUrl, string saveFilePath, int expireInSeconds = 3600);

        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        Task DownloadAsync(string rawUrl, string saveFilePath);

        /// <summary>
        /// 私有文件下载
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="expireInSeconds"></param>
        /// <returns></returns>
        Task DownloadPrivateAsync(string rawUrl, string saveFilePath, int expireInSeconds = 3600);
    }
}
