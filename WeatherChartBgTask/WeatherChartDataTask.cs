using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace WeatherChartBgTask
{
    /// <summary>
    ///天気図情報取得タスク
    /// </summary>
    public sealed class WeatherChartDataTask
    {
        private const string CHART_BASE_URL = "http://www.jma.go.jp/jp/g3";
        private const string CHART_COLOR_JS_URL = "http://www.jma.go.jp/jp/g3/hisjs/jp_c.js";
        private const string CHART_BW_JS_URL = "http://www.jma.go.jp/jp/g3/hisjs/jp.js";

        /// <summary>
        /// 天気図画像情報を取得する
        /// </summary>
        /// <returns></returns>
        public static IAsyncOperation<IReadOnlyList<ChartImageItem>> GetChartItemList(bool isColor)
        {
            return getChartItemListInternal(isColor).AsAsyncOperation();
        }

        /// <summary>
        /// 天気図画像情報を取得する（内部処理用）
        /// </summary>
        /// <returns></returns>
        private static async Task<IReadOnlyList<ChartImageItem>> getChartItemListInternal(bool isColor)
        {
            string jsUrl = CHART_BW_JS_URL;
            if (isColor)
            {
                jsUrl = CHART_COLOR_JS_URL;
            }

            List<ChartImageItem> itemList = new List<ChartImageItem>();
            string json = await getHttpTextInternal(jsUrl);
            string[] lineList = json.Split('\n');
            foreach (string line in lineList)
            {
                string fileName = getFileNameFromJsonData(line);
                string title = getImageTitleFromJsonData(line);
                if (!string.IsNullOrEmpty(fileName))
                {
                    itemList.Add(new ChartImageItem()
                    {
                        ImageUrl = getImageUrl(CHART_BASE_URL, fileName),
                        ImageTitle = title,
                    });

                    if (title.Contains("実況天気図"))
                    {
                        //実況天気図は最新の1件のみを使用するためほかは追加しない
                        break;
                    }
                }
            }

            itemList.Reverse(); //時系列順にするため一番古いものが最初に来るようにする
            return itemList.ToArray();
        }
        
        /// <summary>
        /// 天気図画像URLを取得する
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string getImageUrl(string baseUrl, string fileName)
        {
            return String.Format("{0}/{1}", baseUrl, fileName);
        }

        /// <summary>
        /// 天気図画像列挙データからファイル名を取得する
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string getFileNameFromJsonData(string line)
        {
            string[] splitItems = line.Split('"');
            if (splitItems.Length < 4)
            {
                return "";
            }

            return splitItems[3];
        }

        /// <summary>
        /// 天気図画像列挙データから表示タイトルを取得する
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string getImageTitleFromJsonData(string line)
        {
            string[] splitItems = line.Split('"');
            if (splitItems.Length < 4)
            {
                return "";
            }

            return splitItems[1];
        }

        /// <summary>
        /// 指定したURLからデータをダウンロードし文字列として返す
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IAsyncOperation<string> GetHttpText(string url)
        {

            return getHttpTextInternal(url).AsAsyncOperation();
        }

        /// <summary>
        /// 指定したURLからデータをダウンロードし文字列として返す
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<string> getHttpTextInternal(string url)
        {
            string text = "";
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage message = await httpClient.GetAsync(new Uri(url));
                    text = await message.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("getHttpText e=" + e.Message);
                text = "";
            }

            return text;
        }

        /// <summary>
        /// 指定したURLからデータをダウンロードしファイルに保存後、ファイル情報を返す
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static IAsyncOperation<StorageFile> GetHttpFile(string url, StorageFolder saveFolder, string fileName)
        {
            return getHttpFileInternal(url, saveFolder, fileName).AsAsyncOperation();
        }

        /// <summary>
        /// 指定したURLからデータをダウンロードしファイルに保存後、ファイル情報を返す
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static async Task<StorageFile> getHttpFileInternal(string url, StorageFolder saveFolder, string fileName)
        {
            StorageFile saveFile = null;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    byte[] data = await httpClient.GetByteArrayAsync(new Uri(url));
                    saveFile = await saveFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(saveFile, data);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("getHttpFile e=" + e.Message);
                return null;
            }

            return saveFile;
        }

    }
}
