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
using System.Windows.Shapes;

namespace AllClouds
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }


        private void Test_Click(object sender, RoutedEventArgs e)
        {
            string diskletter = ComboBoxUI.Text;
            MountFS mountfs = new MountFS();
            mountfs.Mount(diskletter);
            Test.Visibility = Visibility.Hidden;
            Test2.Visibility = Visibility.Visible;
        }

        private void Test2_Click(object sender, RoutedEventArgs e)
        {
            string diskletter = ComboBoxUI.Text;
            MountFS mountfs = new MountFS();
            mountfs.Unmount(diskletter);
            Test.Visibility = Visibility.Visible;
            Test2.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MountFS mountfs = new MountFS();
            ComboBoxUI.ItemsSource = mountfs.FreeDiskLetter();
        }
    }
}
