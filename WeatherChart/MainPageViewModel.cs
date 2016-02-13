using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeatherChartBgTask;

namespace WeatherChart
{
    /// <summary>
    /// メイン画面ビュークラス
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 読み込み後の反映待ち時間
        /// </summary>
        private const int DEF_INIT_WAIT_TIME = 1000;

        /// <summary>
        /// フォルダ・ファイルリスト
        /// </summary>
        private ObservableCollection<PivotItem> m_itemList = new ObservableCollection<PivotItem>();

        /// <summary>
        /// コマンドバーに表示するタイトル
        /// </summary>
        private string m_title = "";

        /// <summary>
        /// データ読込中かどうか
        /// </summary>
        private bool m_isLoading = true;

        /// <summary>
        /// フォルダ・ファイルリスト
        /// </summary>
        public ObservableCollection<PivotItem> ItemList
        {
            get
            {
                return m_itemList;
            }
            set
            {
                if (value != m_itemList)
                {
                    m_itemList = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                if (value != m_title)
                {
                    m_title = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// データ読込中かどうか
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return m_isLoading;
            }
            set
            {
                if (value != m_isLoading)
                {
                    m_isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// データをすべてクリアーする
        /// </summary>
        /// <returns></returns>
        public async Task<int> InitClear()
        {
            return await InitClear(DEF_INIT_WAIT_TIME);
        }

        /// <summary>
        /// データをすべてクリアーする
        /// </summary>
        /// <param name="waitTime">処理待ち時間</param>
        /// <returns></returns>
        public async Task<int> InitClear(int waitTime)
        {
            this.ItemList.Clear();
            this.IsLoading = true;
            this.Title = createTitleName("");
            if (waitTime > 0)
            {
                //ピボット切り替え直後にクリアーすると画像が表示されないことがあるため少し待つ
                await Task.Delay(waitTime);
            }

            return waitTime;
        }

        /// <summary>
        /// データ取得
        /// </summary>
        public async void Init()
        {
            if (m_itemList.Count > 0)
            {
                await this.InitClear();
            }

            PivotItem[] nowcastList = await GetChartItemList();
            if (nowcastList == null || nowcastList.Length == 0)
            {
                initError();
                return;
            }
            foreach (PivotItem item in nowcastList)
            {
                this.ItemList.Add(item);
            }

            this.Title = createTitleName(DateTime.Now.ToString("MM/dd HH:mm") + "取得");
        }

        /// <summary>
        /// 取得失敗表示
        /// </summary>
        private void initError()
        {
            this.Title = createTitleName("データ取得エラー");
            this.IsLoading = false;
        }

        /// <summary>
        /// タイトルバーに表示する文字列を生成する
        /// </summary>
        /// <param name="addText">アプリ名の後ろにつける文字列（不要なときは空文字を指定）</param>
        /// <returns></returns>
        private string createTitleName(string addText)
        {
            string title = Windows.ApplicationModel.Package.Current.DisplayName;
            if (!string.IsNullOrEmpty(addText))
            {
                title += " - " + addText;
            }

            return title;
        }

        /// <summary>
        /// 天気図画像情報リストを取得する
        /// </summary>
        /// <returns></returns>
        private async Task<PivotItem[]> GetChartItemList()
        {
            List<PivotItem> pivotItemList = new List<PivotItem>();
            IReadOnlyList<ChartImageItem> chartItemList = await WeatherChartDataTask.GetChartItemList(isUseColor());
            foreach (ChartImageItem chartItem in chartItemList)
            {
                pivotItemList.Add(new PivotItem()
                {
                    Name = chartItem.ImageTitle,
                    ImageUrl = chartItem.ImageUrl,
                });
            }

            return pivotItemList.ToArray();
        }

        /// <summary>
        /// カラー表示を行うかどうか
        /// </summary>
        /// <returns></returns>
        private bool isUseColor()
        {
            return AppSettings.Current.UseColorChart;
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