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

        /// <summary>
        /// タイル更新間隔時間（分）
        /// </summary>
        public int TileUpdateTime
        {
            get { return GetValue<int>(60, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 中タイルの横オフセット
        /// </summary>
        public int MiddleTileHorizontalOffset
        {
            get { return GetValue<int>(0, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 中タイルの縦オフセット
        /// </summary>
        public int MiddleTileVerticalOffset
        {
            get { return GetValue<int>(0, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 中タイルの拡大率
        /// </summary>
        public float MiddleTileZoomFactor
        {
            get { return GetValue<float>(1.0f, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 横長タイルの横オフセット
        /// </summary>
        public int WideTileHorizontalOffset
        {
            get { return GetValue<int>(0, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 横長タイルの縦オフセット
        /// </summary>
        public int WideTileVerticalOffset
        {
            get { return GetValue<int>(0, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 横長タイルの拡大率
        /// </summary>
        public float WideTileZoomFactor
        {
            get { return GetValue<float>(1.0f, ContainerType.Roaming); }
            set
            {
                SetValue(value, ContainerType.Roaming);
                OnPropertyChanged();
            }
        }

    }
}