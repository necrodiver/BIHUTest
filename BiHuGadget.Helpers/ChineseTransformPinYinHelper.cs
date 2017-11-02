using BiHuGadget.Models;
using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BiHuGadget.Helpers
{
    public class ChineseTransformPinYinHelper
    {
        public static List<KVModel> TransFormUserEmailList(List<string> strList)
        {
            var list = TransfromPinYin(strList);
            list = list.ConvertAll(l => new KVModel { Key = l.Key, Value = l.Value + Settings.EmailOrg });
            return list;
        }
        /// <summary>
        /// 汉字转为拼音，针对汉字组成的数组
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static List<KVModel> TransfromPinYin(List<string> strList)
        {
            if (strList == null)
                return null;
            List<KVModel> pinyinList = new List<KVModel>();
            foreach (var item in strList)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < item.Length; i++)
                {
                    if (ChineseChar.IsValidChar(item[i]))
                    {
                        ChineseChar cc = new ChineseChar(item[i]);
                        var pinyins = cc.Pinyins.ToList();
                        sb.Append(pinyins[0]);
                    }
                    else
                        break;
                }
                pinyinList.Add(new KVModel
                {
                    Key = item,
                    Value = sb.ToString()
                });
            }
            if (pinyinList != null)
                pinyinList = pinyinList.ConvertAll<KVModel>(p => new KVModel { Key = p.Key, Value = Regex.Replace(p.Value, @"\d", "").ToLower() });
            //pinyinList.ConvertAll(p => Regex.Replace(p.Value, @"\d", "").ToLower());
            return pinyinList;
        }
        /// <summary>
        /// 汉字转拼音，针对汉字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TransForPyinYin(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                ChineseChar cc = new ChineseChar(str[i]);
                var pinyins = cc.Pinyins.ToList();
                sb.Append(pinyins[0]);
            }
            return sb.ToString();
        }
    }

}
