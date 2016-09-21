using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Rock.Common.Encryption
{
    public class HashCrypt
    {
        /// <summary>
        /// MD5加密 编码类型为默认
        /// </summary>
        /// <param name="value">需要加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string value)
        {
            return MD5Encrypt(value, Encoding.Default);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value">需要加密的字符串</param>
        /// <param name="charencoder">指定对字符串的编码类型</param>
        /// <returns></returns>
        public static string MD5Encrypt(string value, Encoding charencoder)
        {
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            byte[] data = md.ComputeHash(charencoder.GetBytes(value));
            return Convert.ToBase64String(data);
        }

        public static string MD5(string value)
        {
            return MD5(value, Encoding.Default);
        }

        public static string MD5(string value, Encoding charencoder)
        {
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            byte[] bytesOfStr = md.ComputeHash(charencoder.GetBytes(value));
            int bLen = bytesOfStr.Length;
            StringBuilder pwdBuilder = new StringBuilder(32);
            for (int i = 0; i < bLen; i++)
            {
                // 格式化md5 hash 字节数组所用的格式（两位小写16进制数字）   
                pwdBuilder.Append(bytesOfStr[i].ToString("x2"));
            }
            return pwdBuilder.ToString();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string value)
        {
            return SHA1Encrypt(value, Encoding.Default);
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charencoder"></param>
        /// <returns></returns>
        public static string SHA1Encrypt(string value, Encoding charencoder)
        {
            System.Security.Cryptography.SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(charencoder.GetBytes(value));
            StringBuilder sbText = new StringBuilder();
            foreach (var ibyte in data)
            {
                sbText.AppendFormat("{0:x2}", ibyte);
            }
            return sbText.ToString();
        }

        public static string SHA1(string value)
        {
            return SHA1(value, Encoding.Default);
        }

        public static string SHA1(string value, Encoding charencoder)
        {
            System.Security.Cryptography.SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = charencoder.GetBytes(value);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "");
            return str_sha1_out.ToLower();
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charencoder"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string value, Encoding charencoder)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //公钥
            string publickey = string.Empty;

            rsa.FromXmlString(publickey);
            byte[] data = rsa.Encrypt(charencoder.GetBytes(value), false);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charencoder"></param>
        /// <returns></returns>
        public static string RSADecrypt(string value, Encoding charencoder)
        {
            //私钥
            string privatekey = string.Empty;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);
            byte[] data = rsa.Decrypt(Convert.FromBase64String(value), false);
            return charencoder.GetString(data);
        }

    }
}
