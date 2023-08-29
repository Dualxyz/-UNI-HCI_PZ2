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

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkDataView.xaml
    /// </summary>
    public partial class NetworkDataView : UserControl
    {
        public NetworkDataView()
        {
            InitializeComponent();
        }

        private void KeyBoard_input(object sender, MouseButtonEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            VirtualKeyboard keyboardWindow = new VirtualKeyboard(textbox, Window.GetWindow(this));
            if (keyboardWindow.ShowDialog() == true)
                textbox.Text = keyboardWindow.Result;
        }



    }
}
