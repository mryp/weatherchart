using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace WeatherChart
{
    public class SettingPageModelView : INotifyPropertyChanged
    {
        private bool m_useColor = true;
        private string m_versionInfo = "";

        /// <summary>
        /// カラー天気図を使用するかどうか
        /// </summary>
        public bool UseColorChart
        {
            get
            {
                return m_useColor;
            }
            set
            {
                if (value != m_useColor)
                {
                    AppSettings.Current.UseColorChart = value;
                    m_useColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// バージョン情報
        /// </summary>
        public string VersionInfo
        {
            get
            {
                return m_versionInfo;
            }
            set
            {
                if (value != m_versionInfo)
                {
                    m_versionInfo = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            this.UseColorChart = AppSettings.Current.UseColorChart;

            PackageVersion versionInfo = Package.Current.Id.Version;
            this.VersionInfo = String.Format("{0} version {1}.{2}.{3}.{4}"
                , Package.Current.DisplayName
                , versionInfo.Major, versionInfo.Minor, versionInfo.Build, versionInfo.Revision);
        }

        #region INotifyPropertyChanged member

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
