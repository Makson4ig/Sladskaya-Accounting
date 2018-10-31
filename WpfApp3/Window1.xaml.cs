using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;

namespace WpfApp3
{
    public partial class Window1 : Window
    {
        string connection;
        public Window1()
        {
            InitializeComponent();
            connection = ConfigurationManager.ConnectionStrings["WpfApp3.Properties.Settings.ConnectionString"].ConnectionString;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection Connect = new MySqlConnection(connection); // Строка подключения к базе данных
            using (MySqlCommand command = new MySqlCommand(@"select max(id) from t", Connect)) // Переменная выполнения команды 
            {
                try
                {
                    Connect.Open(); // Открываем базу данных 
                    MySqlDataReader reader = command.ExecuteReader(); // Переменная чтения запроса выполненного переменной command
                    reader.Read(); // Читаем 
                    var id = reader.GetValue(0).ToString(); // Переменная для добавления id в Базу данных, присваем значение максимально id в Базе данных 
                    if (id == "") id = "0"; // Проверяем если значение пустое, то присваиваем 0, так как в базе пусто
                    reader.Close(); // Закрываем чтение 

                    Connect.Execute("INSERT INTO t (ID,Fam,Thing,Time) VALUES (" + (Convert.ToInt16(id) + 1) + ",'" + TextFam.Text + "','" + TextThing.Text + "','" + DateTime.Now + "')");// Выполняем запрос на добавление id, фамилии, предмета, текущие Даты и Время.
                    Connect.Close(); // Закрываем базу данных 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Connect.Close();
                }
            }
            this.Hide(); // Закрываем окно 
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

    }
}
