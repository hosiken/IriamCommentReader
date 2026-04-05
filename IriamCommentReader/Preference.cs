using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IriamCommentReader
{
    public class PreferenceBase
    {
        protected const string ReturnStr = "{\\n}";

        // Windows API の宣言（Unicode 対応）
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);

        // int 読み込み用 API（数値の場合はこちらを使えます）
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern uint GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName);

        // INI ファイルのパス (実行ファイルと同じフォルダ、拡張子だけ .ini)
        private static string IniFilePath
        {
            get
            {
                // Application.ExecutablePath は実行ファイルのパスを返す
                return Path.ChangeExtension(Application.ExecutablePath, ".ini");
            }
        }

        protected static void WriteStr(string section, string key, string value)
        {
            value = value.Replace("\r\n", ReturnStr).Replace("\r", ReturnStr).Replace("\n", ReturnStr);
            WritePrivateProfileString(section, key, value, IniFilePath);
        }

        /// <summary>
        /// 指定したセクション、キーの値を INI ファイルから読み込みます。
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="key">キー名</param>
        /// <param name="defaultValue">キーが存在しない場合の既定値</param>
        /// <returns>読み込んだ文字列</returns>
        protected static string ReadStr(string section, string key, string defaultValue)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, IniFilePath);
            return sb.ToString().Replace(ReturnStr, "\r\n");
        }

        #region int の読み書き

        /// <summary>
        /// INI ファイルに int 値を書き込みます。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <param name="value">保存する int 値</param>
        protected void WriteInt(string section, string key, int value)
        {
            WritePrivateProfileString(section, key, value.ToString(), IniFilePath);
        }

        /// <summary>
        /// INI ファイルから int 値を読み込みます。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <param name="defaultValue">キーが存在しない場合の既定値</param>
        /// <returns>読み込んだ int 値</returns>
        protected static int ReadInt(string section, string key, int defaultValue)
        {
            // GetPrivateProfileInt は uint を返すのでキャストしています
            return (int)GetPrivateProfileInt(section, key, defaultValue, IniFilePath);
        }

        #endregion

        #region float の読み書き

        /// <summary>
        /// INI ファイルに float 値を書き込みます。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <param name="value">保存する float 値</param>
        protected void WriteFloat(string section, string key, float value)
        {
            // 小数点の区切り文字を常にドットにするため InvariantCulture を利用
            WritePrivateProfileString(section, key, value.ToString(CultureInfo.InvariantCulture), IniFilePath);
        }

        /// <summary>
        /// INI ファイルから float 値を読み込みます。
        /// </summary>
        /// <param name="key">キー名</param>
        /// <param name="defaultValue">キーが存在しない場合の既定値</param>
        /// <returns>読み込んだ float 値</returns>
        protected static float ReadFloat(string section, string key, float defaultValue)
        {
            // 一旦文字列として読み込み、float にパースします
            string strValue = ReadStr(section, key, defaultValue.ToString(CultureInfo.InvariantCulture));
            if (float.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
            return defaultValue;
        }

        #endregion

        protected void WriteBool(string section, string key, bool value)
        {
            WriteInt(section, key, value ? 1 : 0);
        }

        protected static bool ReadBool(string section, string key, bool defaultValue)
        {
            return ReadInt(section, key, defaultValue ? 1 : 0) == 0 ? false : true;
        }

    }

    public class Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect() : this(0, 0, 0, 0) { }
    }

    public class Preference : PreferenceBase
    {
        public static Preference Instance { get; set; } = new Preference();

        public static readonly List<string> ModelWhiteList = new List<string>()
        {
            "gpt-5.4", "gpt-5.2", "gpt-5.1", "gpt-5", "gpt-5-chat-latest", "gpt-4.1", "gpt-4o-2024-11-20", "o3", "o1"
        };
        public static readonly List<string> MiniModelWhiteList = new List<string>()
        {
            "gpt-5.4-mini", "gpt-5.4-nano", "gpt-5-mini", "gpt-5-nano", "gpt-4.1-mini", "gpt-4.1-nano", "gpt-4o-mini", "o4-mini", "o3-mini"
        };

        private const int DefaultAutoExecInterval = 60;
        private const string DefaultModel = "gpt-5.4";
        private const string DefaultMiniModel = "gpt-5.4-mini";
        private const string DefaultSystemPrompt = "テキストを文字起こししてほしいです。読み上げソフトに渡すため、追加された分だけを、文字起こししてください。\r\n\r\nフォーマット:\r\n- 原則として名前 + メッセージというフォーマットですので、nameとcommentを分けてください\r\n- システムメッセージはnameなし(空文字列)でcommentのみとする\r\n- 絵文字は省略する、若葉マークに注意\r\n- 水色や灰色などは名前です。黒色はチャット本文です\r\n- 3点リーダーは…に統一する\r\n- 新しいチャットは下に追加されていく\r\n- 【直近読み上げたテキスト】が指定されている場合は…\r\n    - 同じテキストを再出力してはいけません。\r\n    - 【直近読み上げたテキスト】よりも下の行に追加された続きのみ出力してください\r\n    - 新規の行が下にない(レスポンスに出力すべきテキストがない場合)場合は、空jsonを出力\r\n    - 【直近読み上げたテキスト】が読み取ったテキストにない場合は、ログが画面外に流されたものと見なして全文読んでください";
        private const string DefaultInitPrompt = "【直近読み上げたテキスト】\r\n(ありません。この指示が初回ですので全文取得します)";
        private const string DefaultPrompt = "【直近読み上げたテキスト】\r\n{{text}}\r\n\r\n(これより前の行は読み上げ済みなので、レスポンスに出力しないてください)";
        private const string DefaultBouyomiURL = "http://localhost:50080/Talk";
        private const string DefaultBouyomiParam = "?text={{text}}";
        private const float DefaultSimilarity = 0.5f;
        private const float DefaultChatSimilarity = 0.5f;
        private const int DefaultSimilarRetryInterval = 1;
        private const int DefaultImageResizeWidth = 400;

        public string APIKey { get; set; }
        public string Model { get; set; }
        public string MiniModel { get; set; }
        public float Temperature { get; set; }
        public float TopP { get; set; } = 0.0f;
        public string SystemPrompt { get; set; }
        public string InitPrompt { get; set; }
        public string Prompt { get; set; }
        public bool SkipNameAll { get; set; }
        public bool SkipName { get; set; }
        public bool SimilarOnly { get; set; }
        public float Similarity { get; set; }
        public bool SimilarityRetryEnable { get; set; }
        public int SimilarRetryInterval { get; set; }
        public bool SimilarityChatSkip { get; set; }
        public float ChatSimilarity { get; set; }
        public bool ImageResizeEnable { get; set; }
        public int ImageResizeWidth { get; set; }
        public string BouyomiURL { get; set; }
        public string BouyomiParam { get; set; }
        public bool AutoExecEnable { get; set; }
        public int AutoExecInterval { get; set; }
        public Rect CaptureRect { get; set; } = new Rect(0, 0, 100, 100);
        public bool UseBase64 { get; set; }
        public int TokenLastDate { get; set; }
        public int LeftTokens { get; set; }
        public int LeftMiniTokens { get; set; }

        // INI ファイルに書き込むセクション名（任意）
        private const string SectionName = "Preference";

        public void ResetToDefault()
        {
            AutoExecEnable = false;
            AutoExecInterval = DefaultAutoExecInterval;
            CaptureRect = new Rect(0, 0, 0, 0);

            Model = DefaultModel;
            MiniModel = DefaultMiniModel;
            Temperature = 0.0f;
            TopP = 0.0f;
            SystemPrompt = DefaultSystemPrompt;
            InitPrompt = DefaultInitPrompt;
            Prompt = DefaultPrompt;
            SkipNameAll = false;
            SkipName = true;
            SimilarOnly = true;
            Similarity = DefaultSimilarity;
            SimilarityRetryEnable = true;
            SimilarRetryInterval = DefaultSimilarRetryInterval;
            SimilarityChatSkip = true;
            ImageResizeEnable = true;
            ImageResizeWidth = DefaultImageResizeWidth;
            ChatSimilarity = DefaultChatSimilarity;
            BouyomiURL = DefaultBouyomiURL;
            BouyomiParam = BouyomiParam;
            UseBase64 = true;
        }

        /// <summary>
        /// 現在のプロパティ値を INI ファイルに保存します。
        /// </summary>
        public void Save()
        {
            // WriteBool(SectionName, "AutoExecEnable", AutoExecEnable);
            WriteInt(SectionName, "AutoExecInterval", AutoExecInterval);
            WriteInt(SectionName, "CaptureRectX", CaptureRect.X);
            WriteInt(SectionName, "CaptureRectY", CaptureRect.Y);
            WriteInt(SectionName, "CaptureRectWidth", CaptureRect.Width);
            WriteInt(SectionName, "CaptureRectHeight", CaptureRect.Height);

            WriteStr(SectionName, "APIKey", APIKey);
            WriteStr(SectionName, "Model", Model);
            WriteStr(SectionName, "MiniModel", MiniModel);
            WriteFloat(SectionName, "Temperature", Temperature);
            WriteFloat(SectionName, "TopP", TopP);
            WriteStr(SectionName, "SystemPrompt", SystemPrompt);
            WriteStr(SectionName, "InitPrompt", InitPrompt);
            WriteStr(SectionName, "Prompt", Prompt);
            WriteBool(SectionName, "SkipNameAll", SkipNameAll);
            WriteBool(SectionName, "SkipName", SkipName);
            WriteBool(SectionName, "SimilarOnly", SimilarOnly);
            WriteFloat(SectionName, "Similarity", Similarity);
            WriteBool(SectionName, "SimilarityRetryEnable", SimilarityRetryEnable);
            WriteInt(SectionName, "SimilarRetryInterval", SimilarRetryInterval);
            WriteBool(SectionName, "SimilarityChatSkip", SimilarityChatSkip);
            WriteFloat(SectionName, "ChatSimilarity", ChatSimilarity);
            WriteBool(SectionName, "ImageResizeEnable", ImageResizeEnable);
            WriteInt(SectionName, "ImageResizeWidth", ImageResizeWidth);
            WriteStr(SectionName, "BouyomiURL", BouyomiURL);
            WriteStr(SectionName, "BouyomiParam", BouyomiParam);
            WriteBool(SectionName, "UseBase64", UseBase64);
        }

        public void SaveTokens()
        {
            WriteInt(SectionName, "TokenLastDate", TokenLastDate);
            WriteInt(SectionName, "LeftTokens", LeftTokens);
            WriteInt(SectionName, "LeftMiniTokens", LeftMiniTokens);
        }

        /// <summary>
        /// INI ファイルから設定値を読み込み、Preference オブジェクトとして返します。
        /// </summary>
        /// <returns>読み込んだ Preference オブジェクト</returns>
        public static Preference Load()
        {
            Preference pref = new Preference();
            pref.AutoExecEnable = ReadBool(SectionName, "AutoExecEnable", false);
            pref.AutoExecInterval = ReadInt(SectionName, "AutoExecInterval", DefaultAutoExecInterval);
            pref.CaptureRect.X = ReadInt(SectionName, "CaptureRectX", 0);
            pref.CaptureRect.Y = ReadInt(SectionName, "CaptureRectY", 0);
            pref.CaptureRect.Width = ReadInt(SectionName, "CaptureRectWidth", 100);
            pref.CaptureRect.Height = ReadInt(SectionName, "CaptureRectHeight", 100);

            pref.APIKey = ReadStr(SectionName, "APIKey", "");
            pref.Model = ReadStr(SectionName, "Model", DefaultModel);
            pref.MiniModel = ReadStr(SectionName, "MiniModel", DefaultMiniModel);
            pref.Temperature = ReadFloat(SectionName, "Temperature", 0.0f);
            pref.TopP = ReadFloat(SectionName, "TopP", 0.0f);
            pref.SystemPrompt = ReadStr(SectionName, "SystemPrompt", DefaultSystemPrompt);
            pref.InitPrompt = ReadStr(SectionName, "InitPrompt", DefaultInitPrompt);
            pref.Prompt = ReadStr(SectionName, "Prompt", DefaultPrompt);
            pref.SkipNameAll = ReadBool(SectionName, "SkipNameAll", false);
            pref.SkipName = ReadBool(SectionName, "SkipName", true);
            pref.SimilarOnly = ReadBool(SectionName, "SimilarOnly", true);
            pref.Similarity = ReadFloat(SectionName, "Similarity", DefaultSimilarity);
            pref.SimilarityRetryEnable = ReadBool(SectionName, "SimilarityRetryEnable", true);
            pref.SimilarRetryInterval = ReadInt(SectionName, "SimilarRetryInterval", DefaultSimilarRetryInterval);
            pref.SimilarityChatSkip = ReadBool(SectionName, "SimilarityChatSkip", true);
            pref.ChatSimilarity = ReadFloat(SectionName, "ChatSimilarity", DefaultChatSimilarity);
            pref.ImageResizeEnable = ReadBool(SectionName, "ImageResizeEnable", true);
            pref.ImageResizeWidth = ReadInt(SectionName, "ImageResizeWidth", DefaultImageResizeWidth);
            pref.BouyomiURL = ReadStr(SectionName, "BouyomiURL", DefaultBouyomiURL);
            pref.BouyomiParam = ReadStr(SectionName, "BouyomiParam", DefaultBouyomiParam);
            pref.UseBase64 = ReadBool(SectionName, "UseBase64", true);

            pref.TokenLastDate = ReadInt(SectionName, "TokenLastDate", 0);
            pref.LeftTokens = ReadInt(SectionName, "LeftTokens", 0);
            pref.LeftMiniTokens = ReadInt(SectionName, "LeftMiniTokens", 0);
            pref.CheckAndUpdateTokenDate();

            if (!ModelWhiteList.Contains(pref.Model))
            {
                pref.Model = DefaultModel;
            }
            if (!MiniModelWhiteList.Contains(pref.MiniModel))
            {
                pref.MiniModel = DefaultMiniModel;
            }
            return pref;
        }

        public bool CheckAndUpdateTokenDate()
        {
            // 1. 今日のお菓子な日付(GMT+0 / UTC)を取得
            DateTime nowUtc = DateTime.UtcNow;

            // 2. 日付を int型 (YYYYMMDD形式) に変換
            // 例: 2023年10月5日 -> 20231005
            int todayInt = (nowUtc.Year * 10000) + (nowUtc.Month * 100) + nowUtc.Day;

            // 3. 保存されている日付と比較
            if (TokenLastDate != todayInt)
            {
                // 4. 今日のお菓子な日付で上書き保存
                TokenLastDate = todayInt;
                LeftTokens = 150000;
                LeftMiniTokens = 1500000;
                SaveTokens();

                return true; // 日付が更新されたことを示す
            }
            else
            {
                return false; // 日付は変更されていない
            }
        }
    }
}
