using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BaiduTranslateApi
{
    class Program
    {
        // 参考文档 ： http://api.fanyi.baidu.com/api/trans/product/apidoc
        static void Main(string[] args)
        {
            var chinesetext = string.Empty;
            var query = "iphone";
            var from = "en";
            var to = "zh";
            var appid = "";// baidu appid 可在百度AI控制台注册
            var salt = "1435660288"; // 随机数
            var key = ""; // baidu key
            var tempstr = appid + query + salt + key;
            var Md5str = Md5Encoding(tempstr);

            // 请求 baidu translate api url
            using (var client = new HttpClient())
            {
                var str = client.GetStringAsync($"https://fanyi-api.baidu.com/api/trans/vip/translate" + $"?q={query}&from={from}&to={to}&appid={appid}&salt={salt}&sign={Md5str}");
                var finaltext = JsonConvert.DeserializeObject<TranslateResult>(str.Result);
                chinesetext = finaltext.trans_result[0].Dst;
            }
            Console.Write(chinesetext);
            Console.ReadKey();
        }

        // MD5 加密方法
        public static string Md5Encoding(string str)
        {
            var pwd = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 

                pwd = pwd + s[i].ToString("x");

            }
            return pwd;
        }
    }

    public class TranslateResult
    {
        public string From { get; set; }

        public string To { get; set; }

        public List<Trans_result> trans_result { get; set; }
    }

    public class Trans_result
    {
        public string Src { get; set; }

        public string Dst { get; set; }
    }
}
