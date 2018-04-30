using System;
using System.Windows;
using System.Windows.Controls;

namespace SBICT.WpfClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            var autoScrollToEnd = true;

            if (sv == null)
                return;

            if (sv.Tag != null)
            {
                autoScrollToEnd = (bool)sv.Tag;
            }

            if (Math.Abs(e.ExtentHeightChange) < 1) // user scroll
            {
                autoScrollToEnd = Math.Abs(sv.ScrollableHeight - sv.VerticalOffset) < 1;
            }
            else // content change
            {
                if (autoScrollToEnd)
                {
                    sv.ScrollToEnd();
                }
            }

            sv.Tag = autoScrollToEnd;
        }
    }
}
