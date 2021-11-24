using System;
using System.Security.Cryptography;
using System.Text;

namespace SFBMS.Common.Algorithm
{
    public static class Algorithm
    {
        /// <summary>
        /// MD5加密(不可逆)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            foreach (byte item in bytes)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="length">随机数长度</param>
        /// <returns></returns>
        public static string GetRandom(int length)
        {
            //byte[] buffer = Guid.NewGuid().ToByteArray();
            //int iSeed = BitConverter.ToInt32(buffer, 0);
            //Random random = new Random(iSeed);
            //Console.WriteLine(random.Next());
            //采用系统当前的硬件信息、进程信息、线程信息、系统启动时间和当前精确时间作为填充因子
            byte[] randoms = new byte[length];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randoms);
            string code = string.Empty;
            for (int i = 0; i < randoms.Length; i++)
            {
                code += (Convert.ToInt32(randoms[i]) % 10).ToString();
            }
            return code;
        }
    }
}
