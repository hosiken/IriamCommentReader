using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IriamCommentReader
{
    public class TextCompare
    {
        // テキストの類似度を比較する
        public static double Compare(string text1, string text2)
        {
            // 正規化
            string norm1 = NormalizeText(text1);
            string norm2 = NormalizeText(text2);

            // Levenshtein距離を計算
            int distance = ComputeLevenshteinDistance(norm1, norm2);

            // 類似度を算出（1.0に近いほど似ている）
            double similarity = 1.0 - (double)distance / Math.Max(norm1.Length, norm2.Length);

            return similarity;
        }

        /// <summary>
        /// 文字列の正規化処理
        /// ひらがな、カタカナ、英数字以外は除去します。
        /// </summary>
        static string NormalizeText(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string output = input;

            // 不要な空白の除去（全角・半角）
            output = output.Replace(" ", "").Replace("　", "");

            // 正規表現でひらがな、カタカナ、英数字以外を削除
            // ひらがな: ぁ-ん, カタカナ: ァ-ヶ, 英数字: a-zA-Z0-9
            output = Regex.Replace(output, "[^ぁ-んァ-ヶa-zA-Z0-9]", "");

            return output;
        }

        /// <summary>
        /// Levenshtein距離（編集距離）を計算するメソッド
        /// </summary>
        static int ComputeLevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
                return string.IsNullOrEmpty(t) ? 0 : t.Length;
            if (string.IsNullOrEmpty(t))
                return s.Length;

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // 初期値の設定
            for (int i = 0; i <= n; i++)
                d[i, 0] = i;
            for (int j = 0; j <= m; j++)
                d[0, j] = j;

            // 動的計画法で距離を計算
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1,   // 削除
                                    d[i, j - 1] + 1),  // 挿入
                        d[i - 1, j - 1] + cost);     // 置換
                }
            }
            return d[n, m];
        }
    }
}
