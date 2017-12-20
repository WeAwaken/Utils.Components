using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 文件上传工厂组件
    /// </summary>
    public static class FileUploadFactory
    {
        /// <summary>
        /// 获取文件上传服务
        /// </summary>
        /// <param name="option">文件上传参数</param>
        /// <returns></returns>
        public static IFileUpload GetServer(FileUploadOption option)
        {
            if (option == null) throw new AppException("上传文件参数不能为空");

            switch (option.Provider)
            {
                case ServerProvider.Xkg:
                    throw new AppException("暂不提供");
                case ServerProvider.Qiniu:
                default:
                    return new FileUpload(option);
            }

        }

    }

    /// <summary>
    /// 文件上传参数
    /// </summary>
    public class FileUploadOption
    {
        public FileUploadOption() { }

        /// <summary>
        /// 生成上传环境[Default:ServerProvider=Qiniu]
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public FileUploadOption(string accessKey,string secretKey)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
        }

        /// <summary>
        /// 文件上传服务商
        /// </summary>
        public ServerProvider Provider { get; set; }

        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }


    }

    /// <summary>
    /// 服务提供商
    /// </summary>
    public enum ServerProvider
    {
        /// <summary>
        /// 七牛云商
        /// </summary>
        Qiniu=0,
        
        /// <summary>
        /// 猩科技自提供
        /// </summary>
        Xkg =1,

        /// <summary>
        /// 七牛云商（私有空间）
        /// </summary>
        SecretQiniu = 2,
    }
}
