/////////////////////////////////////////////////////
//クラス名：Morse
//概要    ：文字列を解析し、
//          モールス符号用構造体Listで保持する
//編集履歴：
// 2015/09/26 Ver.1.0.0
//  新規作成
// 2015/10/11 Ver.1.0.1
//  数字チェックの正規表現に0が抜けていたのを修正
/////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace モールス信号変換機
{
    public class Morse
    {
        /// <summary>
        /// モールス符号
        /// </summary>
        public struct m
        {
            //"トン"
            public const char O = '・';
            //"ツー"
            public const char _ = '－';
            //文字間の"間"（ま） ex. THIS => T_H_I_S と解釈
            public static readonly List<char> SPACE_BETWN_CHARS = new List<char> { '_' };
            //単語間の"間"（ま） ex. THIS IS => THIS / IS と解釈
            public static readonly List<char> SPACE_BETWN_WORDS = new List<char> { ' ', '|', ' ' };
            //和文入力時の欧文開始終了記号
            //ex. オモサ1KGデス => オ|モ|サ|（|1|K|G|）|デ|ス のように日本語英語切替を解釈
            public const char EN_START = '（';
            public const char EN_END = '）';
            public const char PARAGRAPH = '」';
        }
        /// <summary>
        /// 正規表現パターン 和文
        /// </summary>
        /// <example>"^[ %InsertHere% ]+$"</example>
        public const string PTN_JPN = @"あ-んア-ンヰヱｱ-ﾝ゛゜ー、」（） 　";

        /// <summary>
        /// 正規表現パターン 欧文
        /// </summary>
        /// <example>"^[ %InsertHere% ]+$"</example>
        public const string PTN_ENG = @"a-zA-Zａ-ｚＡ-Ｚ\.．,，\?？!！\-‐/／@＠\(\) 　";

        /// <summary>
        /// 正規表現パターン 数字
        /// </summary>
        /// <example>"^[ %InsertHere% ]+$"</example>      
        public const string PTN_NUM = @"0-9０-９";

        /// <summary>
        /// 和英数字区分
        /// </summary>
        public enum ELang
        {
            En = 0,
            Ja = 1,
            Num = 2,
            NonLang = 999,
        }

        /// <summary>
        /// モールス符号の属性
        /// </summary>
        public enum EMrsType
        {
            normal = 0,
            spaceBtwnChars = 1,
            spaceBtwnWords = 2,
            bracketStart = 3,
            bracketEnd = 4,
            errorChar = 999
        }

        /// <summary>
        /// オリジナル文字列
        /// </summary>
        /// <example>ガパ</example>
        public string OriginalString { get; private set; }

        /// <summary>
        /// モールス信号用文字列
        /// </summary>
        /// <example>カ゛ハ゜</example>
        public string StringForMorse { get; private set; }

        /// <summary>
        /// 変換不可文字HashSet
        /// </summary>
        public HashSet<char> NGCharsSet { get; private set; }

        /// <summary>
        /// モールス符号最小単位のItemList
        /// </summary>
        public List<SMorseAtom> Item { get; private set; }
        /// <summary>
        /// モールス符号構成最小単位 構造体
        /// </summary>
        public struct SMorseAtom
        {
            /// <summary>
            /// モールス符号用文字
            /// </summary>
            /// <example>'カ'や'゛'</example>
            public char CharForMorse;

            /// <summary>
            /// 文字ごとのモールス符号List
            /// </summary>
            /// <example> {・－・・} </example>
            public List<char> MorseCodes;

            /// <summary>
            /// 和英数字区分
            /// </summary>
            public ELang Lang;

            /// <summary>
            /// モールス符号の属性
            /// </summary>
            public EMrsType MorseType;

            //構造体コンストラクタ
            public SMorseAtom(char argChar)
                : this() {
                this.CharForMorse = argChar;
                this.MorseType = !char.IsWhiteSpace(argChar) ? EMrsType.normal : EMrsType.spaceBtwnWords;
                this.Lang = getLang(argChar);
                this.MorseCodes = GetMorseFromChar(argChar);
            }
            public SMorseAtom(Morse.EMrsType argMrsType)
                : this() {
                this.CharForMorse = argMrsType == EMrsType.spaceBtwnChars ? new char() :
                                    argMrsType == EMrsType.spaceBtwnWords ? ' ' :
                                    argMrsType == EMrsType.bracketStart ? m.EN_START :
                                    argMrsType == EMrsType.bracketEnd ? m.EN_END : new char();
                this.MorseType = argMrsType;
                this.Lang = ELang.NonLang;
                this.MorseCodes = GetMorseFromChar(argMrsType);
            }
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Morse(string argOriginalString, bool argIsEnglishMode) {
            //オリジナル文字列
            this.OriginalString = argOriginalString;
            //モールス符号用文字列
            var notGoodCharSet = new HashSet<char>();
            string strForMrs = GetStringForMorse(argOriginalString, argIsEnglishMode, out notGoodCharSet);
            this.StringForMorse = strForMrs;
            //モールス符号用文字列変換不可文字HashSet
            this.NGCharsSet = notGoodCharSet;

            //構造体List作成
            //モールス符号Item
            this.Item = GetMorseFromString(strForMrs, argIsEnglishMode);
        }

        /// <summary>
        /// 和英数字判定
        /// </summary>
        /// <param name="argChar"></param>
        /// <returns></returns>
        private static ELang getLang(char argChar) {
            return Regex.IsMatch(argChar.ToString(), "^[" + PTN_ENG + "]+$", RegexOptions.IgnoreCase) ? ELang.En
                 : Regex.IsMatch(argChar.ToString(), "^[" + PTN_NUM + "]+$", RegexOptions.IgnoreCase) ? ELang.Num
                 : ELang.Ja;
        }

        /// <summary>
        /// モールス符号用文字列に変換
        /// </summary>
        /// <param name="argChar"></param>
        /// <returns></returns>
        public static string GetStringForMorse(string argStr, bool argIsEnglish, out HashSet<char> outNGCharSet) {
            outNGCharSet = new HashSet<char>();
            var sb = new StringBuilder();
            foreach (char c in argStr.ToCharArray()) {
                char NGChar = default(char);
                string strForMrs = GetStringForMorse(c, argIsEnglish, out NGChar);
                sb.Append(strForMrs);
                if (NGChar != default(char)) outNGCharSet.Add(NGChar);
            }
            return sb.ToString();
        }
        /// <summary>
        /// モールス符号用文字列に変換
        /// </summary>
        /// <param name="argChar"></param>
        /// <returns></returns>
        /// <example>'が' -> "カ゛"  </example>
        public static string GetStringForMorse(char argChar, bool argIsEnglishMode, out char outNGChar) {
            outNGChar = new char();
            char rtnChar = argChar;

            // 英字
            ELang elang = ELang.NonLang;
            if ((elang = getLang(argChar)) == ELang.En || elang == ELang.Num) {
                rtnChar = Strings.StrConv(rtnChar.ToString(), VbStrConv.Narrow)[0]; //半角
                rtnChar = Strings.StrConv(rtnChar.ToString(), VbStrConv.Uppercase)[0]; //大文字
                return rtnChar.ToString();
            };
            if (argIsEnglishMode) {
                //欧文モードのときは英数字以外を不正文字として扱う
                outNGChar = argChar;
                return "";
            }

            // 特殊ケース
            switch (argChar) {
                case 'ﾞ': return "゛";
                case 'ﾟ': return "゜";
                case '‐': return "-";
                case 'ヰ': return "ヰ";
                case 'ヱ': return "ヱ";
                case '（': return m.EN_START.ToString();
                case '）': return m.EN_END.ToString();
                default: break;
            }

            //全角カタカナ
            rtnChar = Strings.StrConv(rtnChar.ToString(), VbStrConv.Wide)[0];
            rtnChar = Strings.StrConv(rtnChar.ToString(), VbStrConv.Katakana)[0];

            //濁点、半濁点分割カタカナ
            // ex) ガパ => カ゛ハ゜
            //※ 正規表現では [ガ-ゴ] = [ガキギクグケゲコゴ] らしいから濁音のところだけを取り出す
            if (Regex.IsMatch(rtnChar.ToString(), @"[ガギグゲゴザジズゼゾダヂヅデドバビブベボ]")) {
                const int INT_DAKUTEN = (int)'ガ' - (int)'カ'; //濁点文字との文字コード差
                return ((char)((int)rtnChar - INT_DAKUTEN)) + "゛";
            }
            if (Regex.IsMatch(rtnChar.ToString(), @"[パピプペポ]")) {
                const int INT_HAN_DAKUTEN = (int)'パ' - (int)'ハ';
                return ((char)((int)rtnChar - INT_HAN_DAKUTEN)) + "゜";
            }
            //カタカナ特殊ケース
            switch (rtnChar) {
                case 'ッ': return "ツ";
                case 'ァ': return "ア";
                case 'ィ': return "イ";
                case 'ゥ': return "ウ";
                case 'ェ': return "エ";
                case 'ォ': return "オ";
                case 'ヵ': return "カ";
                case 'ャ': return "ヤ";
                case 'ュ': return "ユ";
                case 'ョ': return "ヨ";
                default: break;
            }
            //カタカナ一般
            if (Regex.IsMatch(rtnChar.ToString(), "^[" + PTN_JPN + "]+$", RegexOptions.IgnoreCase)) {
                return rtnChar.ToString();
            }

            //変換できなかった場合は不正文字として返す
            outNGChar = argChar;
            return "";
        }
        public static string GetStringForMorse(EMrsType argEMrsType) {
            switch (argEMrsType) {
                case EMrsType.spaceBtwnChars: return "";　//文字間の"間"
                case EMrsType.spaceBtwnWords: return " "; //単語間の"間"
                case EMrsType.bracketStart: return m.EN_START.ToString(); //和文内欧文始まり"（"
                case EMrsType.bracketEnd: return m.EN_END.ToString(); //和文内欧文終わり"）"

                case EMrsType.normal:
                case EMrsType.errorChar: return "";

                default: return "";
            }
        }

        /// <summary>
        /// モールス符号へ変換（文字列）
        /// </summary>
        /// <param name="argOriginalString"></param>
        /// <param name="getLang"></param>
        /// <returns></returns>
        public static List<SMorseAtom> GetMorseFromString(string argOriginalString, bool argIsEnglish) {
            var rtnListMrsAtm = new List<SMorseAtom>();

            if (argIsEnglish) {
                //欧文入力モード
                //そのまま変換
                foreach (char c in argOriginalString.ToArray()) {
                    rtnListMrsAtm.Add(new SMorseAtom(c));
                }

            } else {
                //和文モード
                // 英字混ざりのときの '（' と '）' に注意する

                bool isEnInputMode = false; // 英字入力中フラグ

                List<char> listOrnlStrChar = argOriginalString.ToArray().ToList();
                for (int i = 0; i < listOrnlStrChar.Count; i++) {
                    var lpMrsAtm = new SMorseAtom(listOrnlStrChar[i]);

                    if (lpMrsAtm.MorseType != EMrsType.spaceBtwnWords) {
                        // 単語間（空白文字）以外

                        if (!isEnInputMode) {
                            // 日本語入力中の場合

                            if (lpMrsAtm.Lang == ELang.Ja || lpMrsAtm.Lang == ELang.Num) {
                                // 日本語だった場合はそのまま追加
                                rtnListMrsAtm.Add(lpMrsAtm);

                            } else {
                                // 英字だった場合
                                if (lpMrsAtm.CharForMorse != m.EN_START) {
                                    // '（'以外の英字だった場合、'（'を入れる
                                    rtnListMrsAtm.Add(new SMorseAtom(m.EN_START));
                                }
                                rtnListMrsAtm.Add(lpMrsAtm);
                                // 英字入力中フラグを立てる
                                isEnInputMode = true;
                            }

                        } else {
                            // 英字入力中の場合

                            if (lpMrsAtm.Lang == ELang.En || lpMrsAtm.Lang == ELang.Num) {
                                // 英字・記号の場合はそのまま追加
                                rtnListMrsAtm.Add(lpMrsAtm);
                                if (lpMrsAtm.CharForMorse == m.EN_END) {
                                    // '）'の場合は英字入力中フラグを下ろす
                                    isEnInputMode = false;
                                }

                            } else {
                                //日本語だった場合は'）'を入れ、英字入力フラグを下ろす
                                rtnListMrsAtm.Add(new SMorseAtom(m.EN_END));
                                rtnListMrsAtm.Add(lpMrsAtm);
                                isEnInputMode = false;
                            }
                        }

                    } else {
                        // 単語間（空白文字）だった場合

                        // 今見ている空白文字終了時の文字Index取得
                        var nextNonSpaceIdx = listOrnlStrChar.FindIndex(i + 1, c => !string.IsNullOrWhiteSpace(c.ToString()));

                        // 文末まで空白の場合（TrimEnd済みであるはずなので想定外のケース）
                        if (nextNonSpaceIdx == -1) {
                            // 最後まで空白文字を入れる（空白文字が連続する場合も対応）
                            do {
                                rtnListMrsAtm.Add(new SMorseAtom(EMrsType.spaceBtwnWords));
                            } while (++i < listOrnlStrChar.Count);
                            // forループを抜ける
                            continue; // 処理結果はbreakと同じ
                        }

                        // 空白文字終了時の文字のSMorseAtomインスタンス
                        var nextNonSpaceMorse = new SMorseAtom(listOrnlStrChar[nextNonSpaceIdx]);

                        // 空白文字終了時に和文欧文が切り替わる場合は'（' か '）'を適宜入れる

                        // 欧文が終わって和文が始まる場合
                        if (isEnInputMode && nextNonSpaceMorse.Lang == ELang.Ja) {
                            // '）'を入れる
                            rtnListMrsAtm.Add(new SMorseAtom(m.EN_END));
                        }

                        // 和文欧文モード切替有無によらず共通
                        // 空白文字を入れる（空白文字が連続する場合はそれらを全部入れておく）
                        do {
                            rtnListMrsAtm.Add(new SMorseAtom(EMrsType.spaceBtwnWords));
                        } while (i + 1 < nextNonSpaceIdx && i++ > 0);

                        // 和文が終わって英文が始まる場合
                        if (!isEnInputMode && nextNonSpaceMorse.Lang == ELang.En) {
                            // '（'を入れる
                            rtnListMrsAtm.Add(new SMorseAtom(m.EN_START));
                        }

                        // 入力モードフラグ更新
                        isEnInputMode = (nextNonSpaceMorse.Lang == ELang.En);
                    }
                }

                if (isEnInputMode) {
                    // 英字入力中のままで終わっていた場合は
                    // '）'を入れる
                    rtnListMrsAtm.Add(new SMorseAtom(m.EN_END));
                }
            }

            //文字間用のスペースを入れる
            for (int i = rtnListMrsAtm.Count - 1; i > 0; i--) {
                var current = rtnListMrsAtm[i];
                var before = rtnListMrsAtm[i - 1];

                if (current.MorseType != EMrsType.spaceBtwnWords
                    && before.MorseType != EMrsType.spaceBtwnWords) {
                    rtnListMrsAtm.Insert(i, new SMorseAtom(EMrsType.spaceBtwnChars));
                }
            }

            return rtnListMrsAtm;
        }

        /// <summary>
        /// モールス符号へ変換（文字単位）
        /// </summary>
        /// <param name="argChar"></param>
        /// <param name="argDoTransStringForMorse"></param>
        /// <returns></returns>
        /// <remarks>モールス符号構造体で使用</remarks>
        public static List<char> GetMorseFromChar(char argChar, bool argDoTransStringForMorse = false) {
            // 文字はモールス符号用文字であることが前提
            // NG:'ガ'、OK:'カ'、'゛'

            // ※変換メソッドを通す処理は入れておくが、
            // モールス符号用の文字毎に変換したいので、基本的に使わない
            if (argDoTransStringForMorse) {
                var rtn = new List<char>();
                char dummy = default(char);
                var strForMorse = GetStringForMorse(argChar, false, out dummy);
                foreach (var c in strForMorse.ToCharArray()) {
                    rtn.AddRange(GetMorseFromChar(c, false));
                }
                return rtn;
            }

            #region モールス符号へ変換

            // モールス符号へ変換
            switch (argChar) {
                case 'A':
                case 'イ': return new List<char> { m.O, m._ };
                case 'ロ': return new List<char> { m.O, m._, m.O, m._ };
                case 'B':
                case 'ハ': return new List<char> { m._, m.O, m.O, m.O };
                case 'C':
                case 'ニ': return new List<char> { m._, m.O, m._, m.O };
                case 'D':
                case 'ホ': return new List<char> { m._, m.O, m.O };
                case 'E':
                case 'ヘ': return new List<char> { m.O };
                case 'ト': return new List<char> { m.O, m.O, m._, m.O, m.O };
                case 'F':
                case 'チ': return new List<char> { m.O, m.O, m._, m.O };
                case 'G':
                case 'リ': return new List<char> { m._, m._, m.O };
                case 'H':
                case 'ヌ': return new List<char> { m.O, m.O, m.O, m.O };
                case 'ル': return new List<char> { m._, m.O, m._, m._, m.O };
                case 'I': return new List<char> { m.O, m.O };
                case 'J':
                case 'ヲ': return new List<char> { m.O, m._, m._, m._ };
                case 'K':
                case 'ワ': return new List<char> { m._, m.O, m._ };
                case 'L':
                case 'カ': return new List<char> { m.O, m._, m.O, m.O };
                case 'M':
                case 'ヨ': return new List<char> { m._, m._ };
                case 'N':
                case 'タ': return new List<char> { m._, m.O };
                case 'O':
                case 'レ': return new List<char> { m._, m._, m._ };
                case 'ソ': return new List<char> { m._, m._, m._, m.O };
                case 'P':
                case 'ツ':
                case 'ッ': return new List<char> { m.O, m._, m._, m.O };
                case 'Q':
                case 'ネ': return new List<char> { m._, m._, m.O, m._ };
                case 'R':
                case 'ナ': return new List<char> { m.O, m._, m.O };
                case 'S':
                case 'ラ': return new List<char> { m.O, m.O, m.O };
                case 'T':
                case 'ム': return new List<char> { m._ };
                case 'U':
                case 'ウ': return new List<char> { m.O, m.O, m._ };
                case 'ヰ': return new List<char> { m.O, m._, m.O, m.O, m._ };
                case 'ノ': return new List<char> { m.O, m.O, m._, m._ };
                case 'オ': return new List<char> { m.O, m._, m.O, m.O, m.O };
                case 'V':
                case 'ク': return new List<char> { m.O, m.O, m.O, m._ };
                case 'W':
                case 'ヤ': return new List<char> { m.O, m._, m._ };
                case 'X':
                case 'マ': return new List<char> { m._, m.O, m.O, m._ };
                case 'Y':
                case 'ケ': return new List<char> { m._, m.O, m._, m._ };
                case 'Z':
                case 'フ': return new List<char> { m._, m._, m.O, m.O };

                case 'コ': return new List<char> { m._, m._, m._, m._ };
                case 'エ': return new List<char> { m._, m.O, m._, m._, m._ };
                case 'テ': return new List<char> { m.O, m._, m.O, m._, m._ };
                case 'ア': return new List<char> { m._, m._, m.O, m._, m._ };
                case 'サ': return new List<char> { m._, m.O, m._, m.O, m._ };
                case 'キ': return new List<char> { m._, m.O, m._, m.O, m.O };
                case 'ユ': return new List<char> { m._, m.O, m.O, m._, m._ };
                case 'メ': return new List<char> { m._, m.O, m.O, m.O, m._ };
                case 'ミ': return new List<char> { m.O, m.O, m._, m.O, m._ };
                case 'シ': return new List<char> { m._, m._, m.O, m._, m.O };
                case 'ヱ': return new List<char> { m.O, m._, m._, m.O, m.O };
                case 'ヒ': return new List<char> { m._, m._, m.O, m.O, m._ };
                case 'モ': return new List<char> { m._, m.O, m.O, m._, m.O };
                case 'セ': return new List<char> { m.O, m._, m._, m._, m.O };
                case 'ス': return new List<char> { m._, m._, m._, m.O, m._ };
                case 'ン': return new List<char> { m.O, m._, m.O, m._, m.O };

                case '゛': return new List<char> { m.O, m.O };
                case '゜': return new List<char> { m.O, m.O, m._, m._, m.O };

                case '1': return new List<char> { m.O, m._, m._, m._, m._ };
                case '2': return new List<char> { m.O, m.O, m._, m._, m._ };
                case '3': return new List<char> { m.O, m.O, m.O, m._, m._ };
                case '4': return new List<char> { m.O, m.O, m.O, m.O, m._ };
                case '5': return new List<char> { m.O, m.O, m.O, m.O, m.O };
                case '6': return new List<char> { m._, m.O, m.O, m.O, m.O };
                case '7': return new List<char> { m._, m._, m.O, m.O, m.O };
                case '8': return new List<char> { m._, m._, m._, m.O, m.O };
                case '9': return new List<char> { m._, m._, m._, m._, m.O };
                case '0': return new List<char> { m._, m._, m._, m._, m._ };

                //欧文記号
                case '.': return new List<char> { m.O, m._, m.O, m._, m.O, m._ };
                case ',': return new List<char> { m._, m._, m.O, m.O, m._, m._ };
                case '?': return new List<char> { m.O, m.O, m._, m._, m.O, m.O };
                case '!': return new List<char> { m._, m.O, m._, m.O, m._, m._ };
                case '-': return new List<char> { m._, m.O, m.O, m.O, m.O, m._ };
                case '/': return new List<char> { m._, m.O, m.O, m._, m.O };
                case '@': return new List<char> { m.O, m._, m._, m.O, m._, m.O };
                case '(': return new List<char> { m._, m.O, m._, m._, m.O };
                case ')': return new List<char> { m._, m.O, m._, m._, m.O, m._ };

                //和文記号
                case 'ー': return new List<char> { m.O, m._, m._, m.O, m._ };
                case '、': return new List<char> { m.O, m._, m.O, m._, m.O, m._ };
                //段落（」）
                case m.PARAGRAPH: return new List<char> { m.O, m._, m.O, m._, m.O, m.O };

                //日本語入力時の英語入力開始
                case m.EN_START: // '('
                    return new List<char> { m._, m.O, m._, m._, m.O, m._ };
                //日本語入力時の英語入力終了
                case m.EN_END: // ')'
                    return new List<char> { m.O, m._, m.O, m.O, m._, m.O };

                //スペース
                case '　':
                case ' ': return m.SPACE_BETWN_WORDS;

                //変換対象外
                default: return new List<char>();
            }

            #endregion

        }
        public static List<char> GetMorseFromChar(EMrsType argEMrsType) {
            switch (argEMrsType) {
                //文字間の"間"
                case EMrsType.spaceBtwnChars: return m.SPACE_BETWN_CHARS;
                //単語間の"間"
                case EMrsType.spaceBtwnWords: return m.SPACE_BETWN_WORDS;
                //和文入力の欧文始まり
                case EMrsType.bracketStart: return GetMorseFromChar(m.EN_START);
                //和文入力の欧文終わり
                case EMrsType.bracketEnd: return GetMorseFromChar(m.EN_END);

                case EMrsType.normal:
                case EMrsType.errorChar:
                default:
                    return new List<char>();
            }
        }
    }
}
