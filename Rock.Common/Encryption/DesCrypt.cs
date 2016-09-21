using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Common.Encryption
{
    /// <summary>
    /// DES 对称加密
    /// 密钥和算法向量都将进行MD5加密后取前8位
    /// 算法向量如果空缺，将采用密钥
    /// </summary>    
    public class DESEncrypt
    {
        public DESEncrypt()
        {

            this.DataEncoding = Encoding.UTF8;
            this.KeyEncoding = Encoding.UTF8;
            this.IVEncoding = Encoding.UTF8;
            this.Format = "{0:x2}";
        }

        /// <summary>
        /// 数据编码格式,默认：UTF8
        /// </summary>
        public Encoding DataEncoding { get; set; }

        /// <summary>
        /// 密钥编码格式,默认：UTF8
        /// </summary>
        public Encoding KeyEncoding { get; set; }

        /// <summary>
        /// 算法向量编码格式,默认：UTF8
        /// </summary>
        public Encoding IVEncoding { get; set; }

        /// <summary>
        /// 加密数据字符串格式化掩码
        /// 默认：{0:x2} 2位16进制小写字符串
        /// 如果需要大写字符串格式为：{0:X2}
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 算法向量
        /// 如果空缺将采用密钥
        /// </summary>
        public string IV { get; set; }

        /// <summary>
        /// 加密字符串数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string data, string key)
        {
            return Encrypt(data, key, null);
        }

        /// <summary>
        /// 加密字符串数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string data, string key, string format)
        {
            DESEncrypt des = new DESEncrypt();
            des.Key = key;

            if (!string.IsNullOrEmpty(format))
                des.Format = format;

            return des.Encrypt(data);
        }


        /// <summary>
        /// 解密字符串数据
        /// </summary>
        /// <param name="data">数据 格式为:{0:x2}</param>
        /// <param name="key">密钥</param>>
        /// <returns>解密密后的字符串数据</returns>
        public static string Decrypt(string data, string key)
        {
            DESEncrypt des = new DESEncrypt();
            des.Key = key;

            return des.Decrypt(data);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>加密后字符串</returns>
        public string Encrypt(string data)
        {
            byte[] endata = this.EncryptData(data);

            StringBuilder result = new StringBuilder();

            foreach (byte b in endata)
                result.AppendFormat(this.Format, b);

            return result.ToString();
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data">数据 格式为:{0:x2}</param>
        /// <returns>解密后字符串</returns>
        public string Decrypt(string data)
        {
            int len;
            len = data.Length / 2;

            byte[] inputByteArray = new byte[len];
            int x, i;

            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(data.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            byte[] dedata = this.DecryptData(inputByteArray);

            return this.DataEncoding.GetString(dedata);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>加密数据</returns>
        public byte[] EncryptData(string data)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;

            inputByteArray = this.DataEncoding.GetBytes(data);

            if (string.IsNullOrEmpty(this.Key))
                throw new Exception("Key不可能空缺");

            string iv = this.IV;
            if (string.IsNullOrEmpty(iv))
                iv = this.Key;

            des.Key = this.KeyEncoding.GetBytes(HashCrypt.MD5Encrypt(this.Key).Substring(0, 8));

            des.IV = this.IVEncoding.GetBytes(HashCrypt.MD5Encrypt(iv).Substring(0, 8));

            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                result = ms.ToArray();
            }

            return result;
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>解密数据</returns>
        public byte[] DecryptData(byte[] data)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            if (string.IsNullOrEmpty(this.Key))
                throw new Exception("Key不可能空缺");

            string iv = this.IV;
            if (string.IsNullOrEmpty(iv))
                iv = this.Key;

            des.Key = this.KeyEncoding.GetBytes(HashCrypt.MD5Encrypt(this.Key).Substring(0, 8));

            des.IV = this.IVEncoding.GetBytes(HashCrypt.MD5Encrypt(iv).Substring(0, 8));

            byte[] result = null;

            using (MemoryStream ms = new MemoryStream())
            {
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                result = ms.ToArray();
            }

            return result;
        }



    }

    public class PublicCryptKeys
    {
        /// <summary>
        /// cookie加密的密钥，初始化向量默认使用此密钥即可
        /// </summary>
        public static readonly string Key = "P+uG5YlM2LcBCkxxwJ1LE2CDFyunRqVK";
    }
}
