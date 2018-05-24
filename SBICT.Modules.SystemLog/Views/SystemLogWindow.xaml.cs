namespace SBICT.Modules.SystemLog.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for SystemLogWindow.xaml.
    /// </summary>
    public partial class SystemLogWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemLogWindow"/> class.
        /// </summary>
        public SystemLogWindow()
        {
            InitializeComponent();
        }
        
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            var autoScrollToEnd = true;

            if (sv == null)
            {
                return;
            }

            if (sv.Tag != null)
            {
                autoScrollToEnd = (bool)sv.Tag;
            }

            // user scroll
            if (Math.Abs(e.ExtentHeightChange) < 1) 
            {
                autoScrollToEnd = Math.Abs(sv.ScrollableHeight - sv.VerticalOffset) < 1;
            }
            else 
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
