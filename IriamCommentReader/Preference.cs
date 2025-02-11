using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

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

    public class Preference : PreferenceBase
    {
        public static Preference Instance { get; set; } = new Preference();

        private const string DefaultModel = "gemini-2.0-flash";
        private const string DefaultSystemPrompt = "ライブ配信アプリの画面キャプチャからリアルタイムでコメントを読み上げるためのテキストをOCR文字起こししてほしいです。読み上げソフトに渡すため、追加された分だけを、文字起こししてください。\r\n\r\nフォーマット:\r\n- AIからの返答やメッセージは一切書かない\r\n- 文字起こしの内容のみ記載\r\n- 絵文字は省略する、若葉マークに注意\r\n- 名前は省略しない\r\n- 水色や灰色などは名前です。黒色はチャット本文です\r\n- チャットにおいて名前とチャットは「 | 」で区切る\r\n    - 「○○さんが」で始まるメッセージはシステムメッセージのですので、「 | 」で区切らない\r\n- 3点リーダーは…に統一する\r\n- 1つのチャットにつき1行\r\n- 新しいチャットは下に追加されていく\r\n- 【直近読み上げたテキスト】が指定されている場合は…\r\n    - 同じテキストを再出力してはいけません。\r\n    - 【直近読み上げたテキスト】よりも下の行に追加された続きのみ出力してください\r\n    - 新規の行が下にない(レスポンスに出力すべきテキストがない場合)場合は、「なし」と出力\r\n    - 【直近読み上げたテキスト】が読み取ったテキストにない場合は、ログが画面外に流されたものと見なして全文読んでください";
        private const string DefaultInitPrompt = "【直近読み上げたテキスト】\r\n(ありません。この指示が初回ですので全文取得します)";
        private const string DefaultPrompt = "【直近読み上げたテキスト】\r\n{{text}}";
        private const string DefaultBouyomiURL = "http://localhost:50080/Talk";
        private const string DefaultBouyomiParam = "?text={{text}}";

        public string APIKey { get; set; }
        public string Model { get; set; }
        public float Temperature { get; set; } = 0.0f;
        public float TopP { get; set; } = 0.0f;
        public string SystemPrompt { get; set; }
        public string InitPrompt { get; set; }
        public string Prompt { get; set; }
        public bool SkipNameAll { get; set; }
        public bool SkipName { get; set; }
        public string BouyomiURL { get; set; }
        public string BouyomiParam { get; set; }

        // INI ファイルに書き込むセクション名（任意）
        private const string SectionName = "Preference";

        public void ResetToDefault()
        {
            Model = DefaultModel;
            Temperature = 0.0f;
            TopP = 0.0f;
            SystemPrompt = DefaultSystemPrompt;
            InitPrompt = DefaultInitPrompt;
            Prompt = DefaultPrompt;
            SkipNameAll = false;
            SkipName = true;
            BouyomiURL = DefaultBouyomiURL;
            BouyomiParam = BouyomiParam;
        }

        /// <summary>
        /// 現在のプロパティ値を INI ファイルに保存します。
        /// </summary>
        public void Save()
        {
            WriteStr(SectionName, "APIKey", APIKey);
            WriteStr(SectionName, "Model", Model);
            WriteFloat(SectionName, "Temperature", Temperature);
            WriteFloat(SectionName, "TopP", TopP);
            WriteStr(SectionName, "SystemPrompt", SystemPrompt);
            WriteStr(SectionName, "InitPrompt", InitPrompt);
            WriteStr(SectionName, "Prompt", Prompt);
            WriteBool(SectionName, "SkipNameAll", SkipNameAll);
            WriteBool(SectionName, "SkipName", SkipName);
            WriteStr(SectionName, "BouyomiURL", BouyomiURL);
            WriteStr(SectionName, "BouyomiParam", BouyomiParam);
        }

        /// <summary>
        /// INI ファイルから設定値を読み込み、Preference オブジェクトとして返します。
        /// </summary>
        /// <returns>読み込んだ Preference オブジェクト</returns>
        public static Preference Load()
        {
            Preference pref = new Preference();
            pref.APIKey = ReadStr(SectionName, "APIKey", "");
            pref.Model = ReadStr(SectionName, "Model", DefaultModel);
            pref.Temperature = ReadFloat(SectionName, "Temperature", 0.0f);
            pref.TopP = ReadFloat(SectionName, "TopP", 0.0f);
            pref.SystemPrompt = ReadStr(SectionName, "SystemPrompt", DefaultSystemPrompt);
            pref.InitPrompt = ReadStr(SectionName, "InitPrompt", DefaultInitPrompt);
            pref.Prompt = ReadStr(SectionName, "Prompt", DefaultPrompt);
            pref.SkipNameAll = ReadBool(SectionName, "SkipNameAll", false);
            pref.SkipName = ReadBool(SectionName, "SkipName", true);
            pref.BouyomiURL = ReadStr(SectionName, "BouyomiURL", DefaultBouyomiURL);
            pref.BouyomiParam = ReadStr(SectionName, "BouyomiParam", DefaultBouyomiParam);
            return pref;
        }
    }
}
