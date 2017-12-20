using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// AES加解密 底部有参考资料
    /// </summary>
    public class CryptoAes
    {
        public static string Encrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            //RijndaelManaged rDel = new RijndaelManaged();
            SymmetricAlgorithm rDel = Aes.Create();            
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string toDecrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            SymmetricAlgorithm rDel = Aes.Create();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #region 参考资料

        /*
         * AES加密CBC模式兼容 PHP、Javascript、Java、C# https://my.oschina.net/Jacker/blog/86383
         * ANY LANG AES:http://outofmemory.cn/code-snippet/35524/AES-with-javascript-java-csharp-python-or-php
         * js 中 AES加密示例 http://www.cnblogs.com/liulun/p/3543774.html
        <script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/rollups/aes.js"></script>
        <script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/rollups/md5.js"></script>
        <script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/components/pad-zeropadding.js"></script>
        <script>
            var data = "mysql_connect('111.111.111.111','root','111111')";
            var key = CryptoJS.enc.Latin1.parse('1111111111111111');
            var iv =    CryptoJS.enc.Latin1.parse('1111111111111111');
            var encrypted = CryptoJS.AES.encrypt(data, key, { iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.ZeroPadding });
            document.write(encrypted);
        </script>         
        */
        #endregion


    }
}
