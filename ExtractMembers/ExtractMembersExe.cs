using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExtractMembers
{
    public class ExtractMembersExe
    {
        public bool Main(string args)
        {
            string stResult = string.Empty;
            
            StreamReader sr = null;

            // 第一引数として渡されたファイルを開く
            try
            {
                sr = new StreamReader(args, Encoding.Default); // Encoding.GetEncoding("Shift_JIS"));
                
                // 抽出した文字列を保存するリスト
                var list = new List<String>();

                // 読み込みできる文字がなくなるまで繰り返す
                while (sr.Peek() >= 0)
                {
                    string l = sr.ReadLine();
                    // ここは良いロジックが浮かばなかったので適当
                    // Propertyも拾うとかしたい
                    bool b1 = IsMatched(l, "^\\s*" + "Public");
                    bool b2 = IsMatched(l, "^\\s*" + "Private");
                    bool b3 = IsMatched(l, "^\\s*" + "Protected");
                    bool b4 = IsMatched(l, "^\\s*" + "Friend");
                    bool b5 = IsMatched(l, "^\\s*" + "Function");
                    bool b6 = IsMatched(l, "^\\s*" + "Sub");
                
                    if (b1 || b2 || b3 || b4 || b5 || b6)
                    {
                        list.Add(l);
                    }
                }
            
                foreach(string tmp in list)
                {
                    Console.WriteLine(tmp);
                }
            }
            catch (FileNotFoundException e)
            {

            } finally
            {
                sr.Close();
            }
            return true;

        }

        /// <summary>
        /// 渡された文字列と正規表現パターンをチェックしてマッチするかを返す
        /// </summary>
        /// <param name="line">ソースコードから読み込んだ1行</param>
        /// <param name="exp">マッチパターン</param>
        /// <returns></returns>
        private bool IsMatched(string line, string exp)
        {
            Regex reg = new Regex(exp);
            Match match = reg.Match(line);
            return match.Success;
        }


    }
}
