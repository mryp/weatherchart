using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherChartBgTask
{
    /// <summary>
    /// 天気図画像情報
    /// </summary>
    public sealed class ChartImageItem
    {
        /// <summary>
        /// 画像URL
        /// </summary>
        public string ImageUrl
        {
            get;
            set;
        }
        
        /// <summary>
        /// 画像タイトル
        /// </summary>
        public string ImageTitle
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChartImageItem()
        {
            this.ImageUrl = "";
            this.ImageTitle = "";
        }
    }
}
