using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util.Common.ExtensionHelper;
using System.Security.Cryptography;
using System.IO;

namespace Util.Common
{
    /// <summary>
    /// 常用加密方法整合
    /// </summary>
    public static class CryptoHelper
    {
        /// <summary>
        /// 封装MD5不可逆加密
        /// 
        /// 已过时:请使用 CryptoHelper.MD5
        /// </summary>
        /// <param name="buff">需要加密的对象</param>
        /// <returns></returns>
        [Obsolete]
        public static byte[] Md5Encrypt(byte[] buff)
        {
            return MD5(buff);
        }
        /// <summary>
        /// 封装MD5不可逆加密
        /// </summary>
        /// <param name="text">需要加密的对象</param>
        /// <param name="encoding">字符集</param>
        /// <returns></returns>
        public static byte[] Md5Encrypt(string text, Encoding encoding)
        {
            byte[] buff = encoding.GetBytes(text);
            return MD5(buff);
        }
        /// <summary>
        /// 获取由MD5加密的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] MD5(byte[] data)
        {
            byte[] result = new byte[0];
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                result = md5.ComputeHash(data, 0, data.Length);
                md5.Clear();
            }
            return result;
        }

        /// <summary>
        /// 生成一对密钥
        /// </summary>
        /// <param name="PublicKey">生成的公钥 用于加密</param>
        /// <param name="PrivateKey">生成的私钥 用于加解密</param>
        public static void BuilderKey(out string PublicKey, out string PrivateKey)
        {
            //生成密钥
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //公钥(加密用)
                PublicKey = rsa.ToXmlString(false);
                //私钥(加、解密用)
                PrivateKey = rsa.ToXmlString(true);
            }
        }

        /// <summary>
        /// RSA对称加密
        /// </summary>
        /// <param name="keyContainerName">密钥容器名称</param>
        /// <param name="buff">需要加密的对象</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static byte[] RSAEncryptDC(string keyContainerName, byte[] buff, bool fOAEP = false)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = keyContainerName;
            byte[] encryptdata;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                encryptdata = Encrypt(rsa, buff, fOAEP);
            }
            return encryptdata;
        }
        /// <summary>
        /// RSA对称加密
        /// </summary>
        /// <param name="keyContainerName">密钥容器名称</param>
        /// <param name="text">需要加密的对象</param>
        /// <param name="encoding">字符集</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static byte[] RSAEncryptDC(string keyContainerName, string text, Encoding encoding, bool fOAEP = false)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = keyContainerName;
            byte[] buff = encoding.GetBytes(text);
            return RSAEncryptDC(keyContainerName, buff, fOAEP);
        }
        /// <summary>
        /// RSA对称解密
        /// </summary>
        /// <param name="keyContainerName">密钥容器名称</param>
        /// <param name="buff">需要解密的对象</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static byte[] RSADecryptDC(string keyContainerName, byte[] buff, bool fOAEP = false)
        {
            CspParameters param = new CspParameters();
            param.KeyContainerName = keyContainerName;
            byte[] decryptdata;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(param))
            {
                decryptdata = Decrypt(rsa, buff, fOAEP);
            }
            return decryptdata;
        }
        /// <summary>
        /// RSA对称解密
        /// </summary>
        /// <param name="keyContainerName">密钥容器名称</param>
        /// <param name="buff">需要解密的对象</param>
        /// <param name="encoding">字符集</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static string RSADecryptDC(string keyContainerName, byte[] buff, Encoding encoding, bool fOAEP = false)
        {
            byte[] encryptdata = RSADecryptDC(keyContainerName, buff, fOAEP);
            return encoding.GetString(encryptdata);
        }

        /// <summary>
        /// RSA非对称加密
        /// </summary>
        /// <param name="Key">公钥或私钥</param>
        /// <param name="buff">需要加密的对象</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string Key, byte[] buff, bool fOAEP = false)
        {
            byte[] encryptdata;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(Key);
                encryptdata = Encrypt(rsa, buff, fOAEP);
            }
            return encryptdata;
        }
        /// <summary>
        /// RSA非对称加密
        /// </summary>
        /// <param name="Key">公钥或私钥</param>
        /// <param name="text">需要加密的对象</param>
        /// <param name="encoding">字符集</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns> 
        public static byte[] RSAEncrypt(string Key, string text, Encoding encoding, bool fOAEP = false)
        {
            byte[] buff = encoding.GetBytes(text);
            return RSAEncrypt(Key, buff, fOAEP);
        }
        /// <summary>
        /// RSA非对称解密
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="buff">需要加密的对象</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static byte[] RSADecrypt(string PrivateKey, byte[] buff, bool fOAEP = false)
        {
            byte[] decryptdata;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(PrivateKey);
                decryptdata = Decrypt(rsa, buff, fOAEP);
            }
            return decryptdata;
        }
        /// <summary>
        /// RSA非对称解密
        /// </summary>
        /// <param name="PrivateKey">私钥</param>
        /// <param name="buff">需要加密的对象</param>
        /// <param name="encoding">字符集</param>
        /// <param name="fOAEP">如果为 true，则使用 OAEP 填充（仅在运行 Microsoft Windows XP 或更高版本的计算机上可用）执行直接的解密；否则，如果为 false，则使用 PKCS#1 1.5 版填充。</param>
        /// <returns></returns>
        public static string RSADecrypt(string PrivateKey, byte[] buff, Encoding encoding, bool fOAEP = false)
        {
            byte[] decryptdata = RSADecrypt(PrivateKey, buff, fOAEP);
            return encoding.GetString(decryptdata);
        }

        static byte[] Encrypt(RSACryptoServiceProvider rsa, byte[] buff, bool fOAEP)
        {
            MemoryStream ms = new MemoryStream();
            int index = 0;
            int length = rsa.KeySize / 8 - 11;
            while (index < buff.Length)
            {
                byte[] temp = rsa.Encrypt(SplitBuff(buff, length, ref index), fOAEP);
                ms.Write(temp, 0, temp.Length);
            }
            return ms.ToArray();
        }
        static byte[] Decrypt(RSACryptoServiceProvider rsa, byte[] buff, bool fOAEP)
        {
            MemoryStream ms = new MemoryStream();
            int index = 0;
            int length = rsa.KeySize / 8;
            while (index < buff.Length)
            {
                byte[] temp = rsa.Decrypt(SplitBuff(buff, length, ref index), fOAEP);
                ms.Write(temp, 0, temp.Length);
            }
            return ms.ToArray();
        }
        static byte[] SplitBuff(byte[] buff, int length, ref int index)
        {
            if (index + length > buff.Length)
            {
                length = buff.Length - index;
            }
            return buff.GetBytes(length, ref index);
        }


        /// <summary>
        /// 对称算法初始化向量
        /// </summary>
        public static byte[] DefaultBIV
        {
            get
            {
                return new byte[] { 0x1 };
            }
        }
        /// <summary>
        /// 默认密钥
        /// </summary>
        public static byte[] DefaultKey
        {
            get
            {
                return new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, };
            }
        }


        /// <summary>  
        /// AES加密(无向量)  
        /// </summary>  
        /// <param name="data">需要加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <param name="size">密钥长度  必须是16或32</param>
        /// <param name="paddingmode">填充模式</param>
        /// <returns>密文</returns>  
        public static byte[] AESEncrypt(byte[] data, byte[] key, int size = 16, PaddingMode paddingmode = PaddingMode.PKCS7)
        {
            Byte[] tKey = new Byte[size];
            int length = key.Length;
            if (length > tKey.Length)
            {
                length = tKey.Length;
            }
            Array.Copy(key, tKey, length);
            int count = data.Length % size;
            int ls = data.Length / size;
            int s = count * size;
            byte[] d = data.GetBytes(s);
            if (s < data.Length)
            {
                d = new byte[32];
            }
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = paddingmode;
                aes.KeySize = 128;
                aes.Key = tKey;
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(d, 0, d.Length);
                        cryptoStream.FlushFinalBlock();
                        byte[] result = mStream.ToArray();
                    }
                }
            }
            return null;
        }

        /// <summary>  
        /// AES解密(无向量)  
        /// </summary>  
        /// <param name="data">被加密的明文</param>  
        /// <param name="key">密钥</param>  
        /// <param name="size">密钥长度  必须是16或32</param>
        /// <param name="paddingmode">填充模式</param>
        /// <returns>明文</returns>  
        public static byte[] AESDecrypt(byte[] data, byte[] key, int size = 16, PaddingMode paddingmode = PaddingMode.PKCS7)
        {
            Byte[] tKey = new Byte[size];
            int length = key.Length;
            if (length > tKey.Length)
            {
                length = tKey.Length;
            }
            Array.Copy(key, tKey, length);
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = paddingmode;
                aes.KeySize = 128;
                aes.Key = tKey;
                using (MemoryStream mStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] tmp = new byte[data.Length + 32];
                        int len = cryptoStream.Read(tmp, 0, data.Length + 32);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return ret;
                    }
                }
            }
        }

        public static byte[] AESEncrypt22(byte[] buff, byte[] key)
        {
            byte[] encrypt = new byte[0];
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Mode = CipherMode.ECB;
                rijndael.Padding = PaddingMode.PKCS7;
                rijndael.Key = key;
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(buff, 0, buff.Length);
                        cryptoStream.FlushFinalBlock();
                        encrypt = mStream.ToArray();
                    }
                }
            }
            return encrypt;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="buff">明文文流</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] RijndaelEncrypt(byte[] buff, byte[] key)
        {
            byte[] encrypt = new byte[0];
            try
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.ECB;
                    rijndael.Padding = PaddingMode.PKCS7;
                    rijndael.Key = key;
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(mStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(buff, 0, buff.Length);
                            cryptoStream.FlushFinalBlock();
                            encrypt = mStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return encrypt;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="buff">密文流</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] RijndaelDecrypt(byte[] buff, byte[] key)
        {
            byte[] decrypt = new byte[0];
            try
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.ECB;
                    rijndael.Padding = PaddingMode.PKCS7;
                    rijndael.Key = key;
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(mStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(buff, 0, buff.Length);
                            cryptoStream.FlushFinalBlock();
                            decrypt = mStream.ToArray();
                        }
                    }
                }
            }
            catch { }
            return decrypt;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="buff">明文文流</param>
        /// <param name="key">密钥</param>
        /// <param name="bIV">对称算法初始化向量</param>
        /// <returns>密文</returns>
        public static byte[] DESEncrypt(byte[] buff, byte[] key, byte[] bIV)
        {
            if (key == null || key.Length != 8)
                key = DefaultKey;
            if (bIV == null)
                bIV = DefaultBIV;
            byte[] encrypt = new byte[0];
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(key, bIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(buff, 0, buff.Length);
                            cStream.FlushFinalBlock();
                            encrypt = mStream.ToArray();
                        }
                    }
                }
                catch { }
            }
            return encrypt;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="buff">密文流</param>
        /// <param name="key">密钥</param>
        /// <param name="bIV">对称算法初始化向量</param>
        /// <returns>明文</returns>
        public static byte[] DESDecrypt(this byte[] buff, byte[] key, byte[] bIV)
        {
            if (key == null || key.Length != 8)
                key = DefaultKey;
            if (bIV == null)
                bIV = DefaultBIV;
            byte[] decrypt = new byte[0];
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(key, bIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(buff, 0, buff.Length);
                            cStream.FlushFinalBlock();
                            decrypt = mStream.ToArray();
                        }
                    }
                }
                catch { }
            }
            return decrypt;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="buff">明文文流</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static byte[] DESEncrypt(byte[] buff, byte[] key)
        {
            byte[] encrypt = new byte[0];
            using (DES des = DES.Create())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        des.Key = key;
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(buff, 0, buff.Length);
                            cStream.FlushFinalBlock();
                            encrypt = mStream.ToArray();
                        }
                    }
                }
                catch { }
            }
            return encrypt;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="buff">密文流</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static byte[] DESDecrypt(this byte[] buff, byte[] key)
        {
            byte[] decrypt = new byte[0];
            using (DES des = DES.Create())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        des.Key = key;
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(buff, 0, buff.Length);
                            cStream.FlushFinalBlock();
                            decrypt = mStream.ToArray();
                        }
                    }
                }
                catch { }
            }
            return decrypt;
        }





        #region 获取由SHA1加密的字符串
        /// <summary>
        /// 获取由SHA1加密的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string EncryptToSHA1(string str)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] str1 = Encoding.ASCII.GetBytes(str);
                byte[] str2 = sha1.ComputeHash(str1);
                sha1.Clear();
                return str2.BytesToString("", "X2");
            }
        }
        #endregion





    }
}
