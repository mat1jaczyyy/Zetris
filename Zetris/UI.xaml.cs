﻿using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Zetris {
    public partial class UI {
        public static bool FreezeEvents = true;

        static string InactiveString, ActiveString, ConfidenceString, ThinkingTimeString;

        Editor Editor = null;

        public UI() {
            InitializeComponent();

            FreezeEvents = false;

            foreach (Style style in Preferences.Styles)
                Style.Items.Add(style);

            Style.SelectedIndex = Preferences.StyleIndex;

            Speed.RawValue = Preferences.Speed;
            Previews.RawValue = Preferences.Previews;
            HoldAllowed.IsChecked = Preferences.HoldAllowed;
            PerfectClear.IsChecked = Preferences.PerfectClear;
            C4W.IsChecked = Preferences.C4W;
            TSDOnly.IsChecked = Preferences.TSDOnly;
            SaveReplay.IsChecked = Preferences.SaveReplay;
            Player.RawValue = Preferences.Player + 1;

#if PUBLIC
            ((StackPanel)Auto.Parent).Children.Remove(Auto);
            ((StackPanel)SaveReplay.Parent).Children.Remove(SaveReplay);
#endif

            Version.Text = $"Zetris-{Assembly.GetExecutingAssembly().GetName().Version.Minor}";

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName) {
                case "ko":
                    InactiveString = "비활성화";
                    ActiveString = "활성화";
                    ConfidenceString = "자신:";
                    ThinkingTimeString = "생각 하는시간:";
                    StyleText.Text = "스타일:";
                    Edit.Content = "플레이 스타일 변경";
                    Speed.Title = "플레이 속도:";
                    Previews.Title = "넥스트 보기 사이즈:";
                    PerfectClear.Content = "퍼펙트 클리어 모드";
                    HoldAllowed.Content = "홀드 사용";
                    C4W.Content = "센터 포와이드";
                    TSDOnly.Content = "TSD만 (20 TSD)";
                    Player.Title = "멀티아케이드:";
                    Gamepad.Content = "게임패드 연결";
                    break;
                    
                case "ja":
                    InactiveString = "停止";
                    ActiveString = "動作中";
                    ConfidenceString = "自信:";
                    ThinkingTimeString = "思考時間:";
                    StyleText.Text = "立ち回り:";
                    Edit.Content = "詳細設定";
                    Speed.Title = "速度:";
                    Previews.Title = "ネクスト可視数:";
                    PerfectClear.Content = "パフェ発見機";
                    HoldAllowed.Content = "ホールド使用";
                    C4W.Content = "中開けREN";
                    TSDOnly.Content = "TSDのみ (TSD20発用)";
                    Player.Title = "ドリームアーケード みんなで:";
                    Gamepad.Content = "コントローラー接続中";
                    break;
                    
                default:
                    InactiveString = "Inactive";
                    ActiveString = "Active";
                    ConfidenceString = "Confidence:";
                    ThinkingTimeString = "Thinking Time:";
                    StyleText.Text = "Style:";
                    Edit.Content = "Edit Styles";
                    Speed.Title = "Speed:";
                    Previews.Title = "Previews:";
                    HoldAllowed.Content = "Hold Allowed";
                    PerfectClear.Content = "Perfect Clear Finder";
                    C4W.Content = "Center 4-Wide";
                    TSDOnly.Content = "TSD Only (for 20 TSD)";
                    Player.Title = "MP Arcade Player:";
                    Gamepad.Content = "Gamepad Connected";
                    break;
            }

            UpdateActive();
            Bot.Start(this);
        }

        bool Active = false;
        public void SetActive(bool state) {
            if (Active != state) {
                Active = state;

                Dispatcher.InvokeAsync(() => UpdateActive());
            }
        }

        public void SetConfidence(string confidence) {
            Dispatcher.InvokeAsync(() => {
                Confidence.Text = $"{ConfidenceString} {confidence}";
                Info.MaxHeight = double.PositiveInfinity;
            });
        }

        public void SetThinkingTime(long time) {
            Dispatcher.InvokeAsync(() => {
                ThinkingTime.Text = $"{ThinkingTimeString} {time}ms";
                Info.MaxHeight = double.PositiveInfinity;
            });
        }

        void UpdateActive() {
            State.Text = Active? ActiveString : InactiveString;
            Edit.IsEnabled = Style.IsEnabled = Speed.Enabled = Previews.Enabled = HoldAllowed.IsEnabled = PerfectClear.IsEnabled = C4W.IsEnabled = TSDOnly.IsEnabled = Player.Enabled = !Active;

            if (Active) Editor?.Close();
            else Info.MaxHeight = 0;
        }

        public void SetGamepadIndex(int index) => Title = $"Zetris [{index}]";
        
        void StyleChanged(object sender, SelectionChangedEventArgs e) {
            if (!FreezeEvents) Preferences.StyleIndex = Style.SelectedIndex;
        }

        void EditClicked(object sender, RoutedEventArgs e) {
            (Editor = new Editor(this)).ShowDialog();

            FreezeEvents = true;

            Style.Items.Clear();

            foreach (Style style in Preferences.Styles)
                Style.Items.Add(style);

            Style.SelectedIndex = Preferences.StyleIndex;
            
            FreezeEvents = false;
        }

        void SpeedChanged(Dial sender, double NewValue) {
            if (!FreezeEvents) Preferences.Speed = (int)Speed.RawValue;
        }

        void PreviewsChanged(Dial sender, double NewValue) {
            if (!FreezeEvents) Preferences.Previews = (int)Previews.RawValue;
        }

        void HoldAllowedChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.HoldAllowed = HoldAllowed.IsChecked == true;
        }

        void PerfectClearChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.PerfectClear = PerfectClear.IsChecked == true;
        }

        void C4WChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.C4W = C4W.IsChecked == true;
        }

        void TSDOnlyChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.TSDOnly = TSDOnly.IsChecked == true;
        }

        void SaveReplayChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.SaveReplay = SaveReplay.IsChecked == true;
        }

        void PlayerChanged(Dial sender, double NewValue) {
            if (!FreezeEvents) Preferences.Player = (int)Player.RawValue - 1;
        }
        
        void AutoChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Preferences.Auto = Auto.IsChecked == true;
        }

        void GamepadChanged(object sender, RoutedEventArgs e) {
            if (!FreezeEvents) Bot.SetGamepad(Gamepad.IsChecked == true);
        }
    }
}
