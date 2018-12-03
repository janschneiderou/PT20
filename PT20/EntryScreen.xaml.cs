using System;
using System.Collections.Generic;
using System.IO;
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

namespace PT20
{
    /// <summary>
    /// Interaction logic for EntryScreen.xaml
    /// </summary>
    public partial class EntryScreen : UserControl
    {
        public MainWindow parent;
        public List<string> users;

        public EntryScreen()
        {
            InitializeComponent();
        }

        public void loaded()
        {
            users = new List<string>();
            GetDirectories();
            

            usersListBox.ItemsSource = users;

        }
        private  void GetDirectories()
        {
            try
            {
                List<string> temp;
                temp = Directory.GetDirectories(MainWindow.usersPath).ToList();
                foreach (string s in temp)
                {
                    int x = s.LastIndexOf("\\");
                    users.Add(s.Substring(x + 1));
                }
                
            }
            catch (UnauthorizedAccessException)
            {
                
            }
        }

        private void userSelected_Click(object sender, RoutedEventArgs e)
        {
            //string usersPath = MainWindow.usersPath + "\\" + nameTextBox.Text;
            string usersPath = System.IO.Path.Combine(MainWindow.usersPath, nameTextBox.Text);
            bool exists = System.IO.Directory.Exists(usersPath);
            if (!exists)
                System.IO.Directory.CreateDirectory(usersPath);

            MainWindow.userDirectory = usersPath;

            //TODO delete when no VR experiment
            MainWindow.ipAddress = ipText.Text;
            MainWindow.portNumber = int.Parse(portText.Text);
        }

        

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nameTextBox.Text = (string)usersListBox.SelectedValue;
        }
    }
}
