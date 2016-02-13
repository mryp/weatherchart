﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WeatherChart
{
    /// <summary>
    /// 設定画面ページ
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        /// <summary>
        /// ビューモデル
        /// </summary>
        public SettingPageModelView ViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingPage()
        {
            this.InitializeComponent();
            this.ViewModel = new SettingPageModelView();
        }

        /// <summary>
        /// このページへ画面遷移した時のイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                this.Frame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += SettingPage_BackRequested;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            this.ViewModel.Init();
            this.DataContext = this.ViewModel;
        }

        /// <summary>
        /// このページから画面遷移する時のイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            SystemNavigationManager.GetForCurrentView().BackRequested -= SettingPage_BackRequested;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        /// <summary>
        /// 前の画面に戻る
        /// </summary>
        /// <returns></returns>
        private bool goBack()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// システムの戻るボタンをおした時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (goBack())
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// キーボード入力イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Back:
                    if (goBack())
                    {
                        args.Handled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// マウスボタンを押下したとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewer_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Pointer pointer = e.Pointer;
            PointerPoint point = e.GetCurrentPoint(sender as ScrollViewer);
            if (point.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Released)
            {
                goBack();
            }
        }
    }
}
