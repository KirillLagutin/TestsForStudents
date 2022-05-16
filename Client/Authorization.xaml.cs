using System.Windows;
using System.Windows.Media;
using DB;

namespace Client
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }
        
        private async void Button_Auth_OnClick(object sender, RoutedEventArgs e)
        {
            string login = TextBox_Login.Text;
            string password = PassBox_Pass.Password;

            if (login.Length < 3)
            {
                TextBox_Login.ToolTip = "Не менее 3-х символов!";
                TextBox_Login.Background = Brushes.Red;
            }
            else if (password.Length < 5)
            {
                PassBox_Pass.ToolTip = "Не менее 5-х символов!";
                PassBox_Pass.Background = Brushes.Red;
            }
            else
            {
                string command = "Select * from tab_authorizations";
                var requestDb = new RequestDb();
                var role = await requestDb.QueryAuthorizationAsync(command, login, password);

                switch (role)
                {
                    case "admin":
                        //MessageBox.Show("Вы авторизовались как администратор!");
                        new AdminWindow().Show();
                        Close();
                        break;
                    case "teacher":
                        //MessageBox.Show("Вы авторизовались как преподаватель!");
                        new AdminWindow().Show();
                        Close();
                        break;
                    case "student":
                        //MessageBox.Show("Вы авторизовались как студент!");
                        new StudentWindow().Show();
                        Close();
                        break;
                    default:
                        MessageBox.Show("Неверные данные авторизации!");
                        break;
                }
            }
        }
    }
}