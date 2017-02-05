using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractMembers
{
    public class ExtractMembersExe
    {
        public bool doExtract(string args)
        { 
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(args, Encoding.Default); // Encoding.GetEncoding("Shift_JIS"));
                
                // 結果を保持するDataTableを定義する
                var dt = new DataTable();
                dt.Columns.Add("Class", Type.GetType("System.String"));
                dt.Columns.Add("Member", Type.GetType("System.String"));

                // ファイルから読み込みできる文字がなくなるまで繰り返す
                while (sr.Peek() >= 0)
                {
                    string l = sr.ReadLine().ToLower();
                    // TODO: ここは良いロジックが浮かばなかったので適当
                    // Propertyも拾うとかしたい
                    bool b1 = IsMatched(l, "public");
                    bool b2 = IsMatched(l, "private");
                    bool b3 = IsMatched(l, "protected");
                    bool b4 = IsMatched(l, "friend");
                    bool b5 = IsMatched(l, "function");
                    bool b6 = IsMatched(l, "sub");
                    bool b7 = IsMatched(l, "property");

                    // どれかに該当すればDataTableに保存する
                    if (b1 || b2 || b3 || b4 || b5 || b6)
                    {
                        var row = dt.NewRow();
                        row["Class"] = Path.GetFileName(args);
                        row["Member"] = l.Trim();
                        dt.Rows.Add(row);
                    }
                }

                // CSVファイルとして出力する
                string dir = ConfigurationManager.AppSettings["CSVExportDir"];
                var d = DateTime.Now.ToString("yyyyMMddhhmmss");
                var filepath = dir + d + "_" + Path.GetFileName(args) + ".csv"; 
                ConvertDataTableToCsv(dt, filepath, true);
            
            }
            catch (Exception e)
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

        /// <summary>
        /// DataTableの内容をCSVファイルに保存する
        /// </summary>
        /// <param name="dt">CSVに変換するDataTable</param>
        /// <param name="csvPath">保存先のCSVファイルのパス</param>
        /// <param name="writeHeader">ヘッダを書き込む時はtrue。</param>
        public void ConvertDataTableToCsv(
            DataTable dt, string csvPath, bool writeHeader)
        {
            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(csvPath, false, enc);

            int colCount = dt.Columns.Count;
            int lastColIndex = colCount - 1;

            //ヘッダを書き込む
            if (writeHeader)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //レコードを書き込む
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    //フィールドの取得
                    string field = row[i].ToString();
                    //"で囲む
                    field = EncloseDoubleQuotesIfNeed(field);
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");
            }

            //閉じる
            sr.Close();
        }

        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }
    }
}
