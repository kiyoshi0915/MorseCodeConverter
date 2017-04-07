/////////////////////////////////////////////////////
//クラス名：Form1.cs
//概要    ：入力された文字列をモールス信号へ変換し、
//        ：表示および再生を行う
//編集履歴：2015/09/26 Ver.1.0.0 新規作成
/////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace モールス信号変換機
{
    public partial class Form1 : Form
    {
        //モールス信号周波数規定値
        private const int C_FREQUENCY_DEFAULT = 800;
        //モールス信号１符号再生ミリ秒規定値
        private const int C_DURATION_DEFAULT = 100;

        //モールス信号周波数
        private int _frequency = C_FREQUENCY_DEFAULT;
        //モールス信号１符号再生ミリ秒
        private int _duration = C_DURATION_DEFAULT;

        /// <summary>
        /// 画面内全コントロール
        /// </summary>
        private List<Control> _allCtrls = null;

        //コンストラクタ
        public Form1() {
            InitializeComponent();
        }

        /// <summary>
        /// 画面ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e) {
            //画面初期化
            this.rdoAutoSelect.Checked = true;
            this.txtInput.Text = "ここに文字を入力してください。";
            this.lblErrMsg.Text = "ここにエラーメッセージが表示されます。";
            this.rtxtStringForMorse.Text = "ここにモールス信号に変換されている文字が表示されます。";
            this.rtxtMorse.Text = "ここにモールス信号が表示されます。";

            //メンバ変数初期化
            _allCtrls = getInputControls(this);

            //イベント
            foreach (TextBox t in _allCtrls.Where(_ => _ is TextBox)) {
                t.Click += textBox_Click;
                t.Enter += textBox_Enter;
                t.Leave += textBox_Leave;
            }
            foreach (TextBox t in new[] { this.txtDuration, this.txtFrequency }) {
                t.Leave += numTextBox_Leave;
            }
        }

        /// <summary>
        /// 入力テキストボックス選択時 全選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private TextBox selectedTextBox = null;
        private void textBox_Click(object sender, EventArgs e) {
            var textBox = (TextBox)sender;
            if (selectedTextBox != textBox) {
                textBox.SelectAll();
            }
            //入力保持
            selectedTextBox = textBox;
        }
        private void textBox_Enter(object sender, EventArgs e) {
            this.txtInput.SelectAll();
        }
        private void textBox_Leave(object sender, EventArgs e) {
            selectedTextBox = null;
        }
        /// <summary>
        /// 数値入力系テキストボックス Leave時 半角数値化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numTextBox_Leave(object sender, EventArgs e) {
            var textBox = (TextBox)sender;
            textBox.Text = Strings.StrConv(textBox.Text, VbStrConv.Narrow);
        }

        /// <summary>
        /// 変換表リンク押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkConvView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            //リンク先に移動したことにする
            linkConvView.LinkVisited = true;
            //ブラウザで開く
            Process.Start("https://ja.wikipedia.org/wiki/%E3%83%A2%E3%83%BC%E3%83%AB%E3%82%B9%E7%AC%A6%E5%8F%B7#.E6.AC.A7.E6.96.87.E3.83.A2.E3.83.BC.E3.83.AB.E3.82.B9.E7.AC.A6.E5.8F.B7");
        }

        /// <summary>
        /// 変換ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConv_Click(object sender, EventArgs e) {

            bool isChkAutoSelect = false;

            try {
                //初期化
                this.lblErrMsg.Text = "";
                this.rtxtMorse.Clear();
                this.rtxtStringForMorse.Clear();

                //画面ロック
                _allCtrls.ForEach(_ => _.Enabled = false);

                //自動判別ラジオボタン最後に戻す用
                isChkAutoSelect = this.rdoAutoSelect.Checked;

                //欧文入力モードフラグ（取得用）
                bool isEnglishMode = false;
                //入力チェック
                string errMsg = "";
                if ((errMsg = inputCheck(out isEnglishMode)) != "") {
                    this.lblErrMsg.Text = errMsg;
                    return;
                }

                //変数格納
                //音の高さ
                int n = 0;
                _frequency = (int.TryParse(this.txtFrequency.Text, out n) && n >= 37 && n <= 32767)
                            ? n : C_FREQUENCY_DEFAULT;
                this.txtFrequency.Text = _frequency.ToString();
                //音の長さ
                _duration = (int.TryParse(this.txtDuration.Text, out n) && n > 0 && n <= 1000)
                             ? n : C_DURATION_DEFAULT;
                this.txtDuration.Text = _duration.ToString();

                //入力文字列
                string inputText = this.txtInput.Text.TrimEnd();
                if (isEnglishMode) {
                    //改行はスペースに（とりあえず今の仕様として）変換
                    inputText = inputText.Replace("\r\n", " ");
                } else {
                    //改行は特殊文字（'」'）に変換
                    inputText = inputText.Replace("\r\n", Morse.m.PARAGRAPH.ToString());
                }

                //Moresインスタンス生成
                var morse = new Morse(inputText, isEnglishMode);

                //画面表示
                for (int i = 0; i < morse.Item.Count; i++) {
                    //モールス信号
                    rtxtMorse.Text += string.Concat(morse.Item[i].MorseCodes);
                    //原文
                    rtxtStringForMorse.Text += string.Concat(morse.Item[i].CharForMorse);
                }

                //音再生
                playMorse(morse);

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            } finally {
                if (isChkAutoSelect) { this.rdoAutoSelect.Checked = true; }
                _allCtrls.ForEach(_ => _.Enabled = true);
            }

            return;
        }

        /// <summary>
        /// 入力系画面コントロール取得
        /// </summary>
        /// <param name="topCtrl"></param>
        /// <returns></returns>
        private List<Control> getInputControls(Control topCtrl) {
            var inputCtrlTypes = new List<Type> { 
                typeof(CheckBox), typeof(TextBox), typeof(Button), typeof(RadioButton) 
            };
            var rtnCtrls = new List<Control>();
            foreach (Control c in topCtrl.Controls) {
                if (inputCtrlTypes.Contains(c.GetType())) { rtnCtrls.Add(c); }
                rtnCtrls.AddRange(getInputControls(c));
            }
            return rtnCtrls;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <returns></returns>
        private string inputCheck(out bool refIsEnglishMode) {
            refIsEnglishMode = true;

            //空欄チェック（文字列の先頭末尾空白可）
            if (this.txtInput.Text.Length == 0) {
                return "文字を入力して下さい。";
            }
            if (string.IsNullOrWhiteSpace(this.txtInput.Text)) {
                return "空白以外の文字を入力して下さい。";
            }

            //改行文字は判定対象外
            string checkString = this.txtInput.Text.Replace("\r\n", "");

            //有効文字チェック
            HashSet<char> NGCharSet = null;
            if (rdoAutoSelect.Checked) {
                //英和自動判別
                //欧文チェック
                Morse.GetStringForMorse(checkString, true, out NGCharSet);
                if (NGCharSet.Count == 0) {
                    refIsEnglishMode = true;
                    this.rdoEnglishMode.Checked = true;
                    return "";
                }

                //和文チェック
                Morse.GetStringForMorse(checkString, false, out NGCharSet);
                if (NGCharSet.Count == 0) {
                    refIsEnglishMode = false;
                    this.rdoJapaneseMode.Checked = true;
                    return "";
                }

                //欧文和文NG時
                return "欧文和文（漢字除く）どちらのパターンにも当てはまっていません";

            } else if (rdoEnglishMode.Checked) {
                //欧文モード選択
                //欧文チェック
                Morse.GetStringForMorse(checkString, true, out NGCharSet);
                if (NGCharSet.Count == 0) {
                    refIsEnglishMode = true;
                    return "";
                }

                //欧文NG時
                return ("アルファベット、数字、記号(一部)のみで入力してください。" + Environment.NewLine
                           + "NG： " + string.Join(",", NGCharSet));

            } else if (rdoJapaneseMode.Checked) {
                //和文モード選択
                //和文チェック
                Morse.GetStringForMorse(checkString, false, out NGCharSet);
                if (NGCharSet.Count == 0) {
                    refIsEnglishMode = false;
                    return "";
                }

                //和文NG時
                return ("ひらがな、カタカナ、アルファベット、数字、記号(一部)のみで入力してください。" + Environment.NewLine
                     + "NG： " + string.Join(",", NGCharSet));

            }

            //※処理はここへは来ない想定
            return "";
        }

        /// <summary>
        /// モールス信号再生
        /// </summary>
        /// <param name="i"></param>
        private void playMorse(Morse argMorse) {
            int strIdx = 0; //モールス用文字列強調進行インデックス
            int mrsIdx = 0; //モールス符号強調進行インデックス

            foreach (var item in argMorse.Item) {
                //一音ずつ逐次再生
                switch (item.MorseType) {
                    case Morse.EMrsType.normal:
                    case Morse.EMrsType.bracketStart:
                    case Morse.EMrsType.bracketEnd:
                        //文字列強調進行
                        emphasisRichText(rtxtStringForMorse, ++strIdx);

                        for (int i = 0; i < item.MorseCodes.Count; i++) {
                            // "トン"、"ツー"
                            int duration = (item.MorseCodes[i] == Morse.m.O)
                                ? _duration : (_duration * 3);

                            //モールス符号強調進行
                            emphasisRichText(rtxtMorse, ++mrsIdx, true);
                            Console.Beep(_frequency, duration);

                            // モールス符号(線、点) 間の"間"
                            if (i < item.MorseCodes.Count - 1)
                                Thread.Sleep(_duration);
                        }
                        break;

                    case Morse.EMrsType.spaceBtwnChars: // 文字間の"間"
                        //モールス符号強調進行
                        emphasisRichText(rtxtMorse, (mrsIdx += item.MorseCodes.Count), true);
                        Thread.Sleep(_duration * 3);
                        break;

                    case Morse.EMrsType.spaceBtwnWords: // 単語間の"間"
                        //文字列強調進行
                        emphasisRichText(rtxtStringForMorse, ++strIdx);

                        //モールス符号強調進行
                        emphasisRichText(rtxtMorse, (mrsIdx += item.MorseCodes.Count), true);
                        Thread.Sleep(_duration * 7);
                        break;

                    case Morse.EMrsType.errorChar:
                        break;

                    default:
                        break;
                }
            }

        }
        /// <summary>
        /// リッチテキストの文字色強調
        /// </summary>
        private void emphasisRichText(RichTextBox rtxt, int colorTextLen, bool isDoEvents = false) {
            //原文の色セット
            rtxt.Select(0, colorTextLen);
            rtxt.SelectionColor = Color.Red;
            if (isDoEvents) Application.DoEvents();
        }

    }
}
