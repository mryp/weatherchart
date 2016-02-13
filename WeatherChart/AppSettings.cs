using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherChart
{
    /// <summary>
    /// アプリケーション設定操作クラス
    /// </summary>
    class AppSettings : AppSettingsBase
    {
        private static readonly AppSettings _current = new AppSettings();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppSettings()
        {
        }

        /// <summary>
        /// 共通で使用するカレントオブジェクト
        /// </summary>
        public static AppSettings Current
        {
            get { return _current; }
        }

        /// <summary>
        /// カラー天気図を使用する
        /// </summary>
        public bool UseColorChart
        {
            get { return GetValue<bool>(true, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

    }
}