using Qiniu.Util;
using System;
using System.IO;
using Qiniu.Http;
using Qiniu.IO.Model;
using Qiniu.IO;
using Qiniu.Common;
using System.Text;
using Qiniu.RS;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 图像上传处理服务(默认Qiniu)
    /// </summary>
    public class FileUpload:IFileUpload
    {
        private static Mac _mac;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        public FileUpload(FileUploadOption option)
        {
            _mac = new Mac(option.AccessKey, option.SecretKey);

        }        

        /// <summary>
        /// 上传[覆盖模式]
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fname"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public string Upload(Stream fileStream, string fname, string bucketName)
        {   
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();

            // 设置要上传的目标空间; "覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            putPolicy.Scope = string.Format("{0}:{1}", bucketName, fname);
                       
            putPolicy.SetExpires(3600);

            string jstr = putPolicy.ToJsonString();

            // 生成上传凭证
            string token = Auth.CreateUploadToken(_mac, jstr);

            FormUploader fu = new FormUploader();

            var result = fu.UploadStream(fileStream, fname, token);

            if (result.Code != 200) {

                throw new AppException(result.Text);
                //return string.Empty;
            
            }

            return fname;
        }

        /// <summary>
        /// 异步上传[覆盖模式]
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="fname"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<string> UploadAsync(Stream fileStream, string fname, string bucketName)
        {
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();

            // 设置要上传的目标空间; "覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            putPolicy.Scope = string.Format("{0}:{1}", bucketName, fname);

            putPolicy.SetExpires(3600);

            string jstr = putPolicy.ToJsonString();

            // 生成上传凭证
            string token = Auth.CreateUploadToken(_mac, jstr);

            FormUploader fu = new FormUploader();

            var result =await fu.UploadStreamAsync(fileStream, fname, token);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }

            return fname;
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public void Delete(string fileName, string bucketName)
        {
            BucketManager bm = new BucketManager(_mac);

            var result = bm.Delete(bucketName, fileName);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
                //return string.Empty;

            }
        }

        /// <summary>
        /// 异步删除单个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task DeleteAsync(string fileName, string bucketName)
        {
            BucketManager bm = new BucketManager(_mac);

            var result =await bm.DeleteAsync(bucketName, fileName);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
                //return string.Empty;

            }
        }

        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public void BatchDelete(string[] fileNames, string bucketName)
        {            
            BucketManager bm = new BucketManager(_mac);
              
            var result = bm.BatchDelete(bucketName, fileNames);

            if (result.Code != 200) {
                throw new AppException(result.Text);
            }
        }

        /// <summary>
        /// 异步批量删除文件
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task BatchDeleteAsync(string[] fileNames, string bucketName)
        {
            BucketManager bm = new BucketManager(_mac);

            var result =await bm.BatchDeleteAsync(bucketName, fileNames);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }
        }

        /// <summary>
        /// 生成授权的下载链接(访问私有空间中的文件时需要使用)
        /// </summary>
        /// <param name="rawUrl">初始链接</param>
        /// <param name="expireInSeconds">下载有效时间(单位:秒)</param>
        /// <returns>包含过期时间的已授权的下载链接</returns>
        public string CreateSignedUrl(string rawUrl,int expireInSeconds = 3600)
        {
            return DownloadManager.CreateSignedUrl(_mac, rawUrl, expireInSeconds);
        }

        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        public void Download(string rawUrl,string saveFilePath)
        {
            // 可公开访问的url，直接下载
            HttpResult result = DownloadManager.Download(rawUrl, saveFilePath);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }
        }

        /// <summary>
        /// 私有文件下载
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="expireInSeconds"></param>
        /// <returns></returns>
        public void DownloadPrivate(string rawUrl, string saveFilePath, int expireInSeconds = 3600)
        {
            string accUrl = DownloadManager.CreateSignedUrl(_mac, rawUrl, expireInSeconds);

            // 接下来可以使用accUrl来下载文件
            HttpResult result = DownloadManager.Download(accUrl, saveFilePath);
            
            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }

        }

        /// <summary>
        /// 下载可公开访问的文件(不适用于大文件)
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        public async Task DownloadAsync(string rawUrl, string saveFilePath)
        {
            // 可公开访问的url，直接下载
            HttpResult result =await DownloadManager.DownloadAsync(rawUrl, saveFilePath);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }
        }

        /// <summary>
        /// 私有文件下载
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <param name="saveFilePath"></param>
        /// <param name="expireInSeconds"></param>
        /// <returns></returns>
        public async Task DownloadPrivateAsync(string rawUrl, string saveFilePath, int expireInSeconds = 3600)
        {
            string accUrl = DownloadManager.CreateSignedUrl(_mac, rawUrl, expireInSeconds);

            // 接下来可以使用accUrl来下载文件
            HttpResult result =await DownloadManager.DownloadAsync(accUrl, saveFilePath);

            if (result.Code != 200)
            {
                throw new AppException(result.Text);
            }

        }
    }
}
