using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using AnalysisCountsSteps.Cores;

namespace AnalysisCountsSteps
{
    public partial class MainWindowAction : Window
    {
        public SqlConnection sqlConnection = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataTable DataTable = null;

        public static string NameText = "";

        public string name, status;
        public int average, bestres, worstres, rank;

        public MainWindowAction()
        {
            InitializeComponent();

            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-0RUJG5Q\SQLEXPRESS;Initial Catalog=AnalysisCountSteps;Integrated Security=True");
            sqlConnection.Open();

            UpdateDataGridAndDataTable("SELECT ФИО,СреднееКолВоШагов,ЛучшийРезультат,ХудшийРезультат FROM [Statistics_]");
        }

        private void TextBox1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)//обработчик события нажатия enter
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    NameText = TextBox1.Text;//запись в переменную текста из textbox
                    UpdateDataGridAndDataTable("SELECT * FROM Statistics_ WHERE ФИО = '" + NameText + "' ");//запрос в бд
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка!" + ex.Message);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)//обработчик события сохранения данных в файл
        {
            if (TextBox1.Text == String.Empty) MessageBox.Show("Для того,чтобы сохранить данные в файл введите Фамилию и Имя пользователя!");
            else
            {
                GetDataFromDataBase();//получаем данные из бд

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.FileName = "Документ";
                dialog.DefaultExt = ".xml";
                dialog.Filter = "xml file (*.xml)|*.xml|json file (*.json)|*.json|csv file (*.csv)|*.csv";

                Nullable<bool> res = dialog.ShowDialog();
                if (res == true)
                {
                    try
                    {   //проверки FilterIndex, который проверяет какой формат выбрал пользователь
                        if (dialog.FilterIndex == 1)
                        {
                            DataTable = ((DataView)DataGrid1.ItemsSource).ToTable();
                            DataTable.TableName = @"Steps";
                            DataTable.WriteXml(dialog.OpenFile());//запись файл в формате xml
                        }
                        else if (dialog.FilterIndex == 2)
                        {
                            ParseDatesToJson dates = new ParseDatesToJson(name, average, bestres, worstres, status, rank);
                            string serialized = JsonConvert.SerializeObject(dates);//сериализация объекта
                            if (serialized.Count() > 1) File.WriteAllText(dialog.FileName.ToString(), serialized, Encoding.GetEncoding(1251));//запись данных в файл
                        }
                        else if (dialog.FilterIndex == 3)
                        {
                            using (var stream = new StreamWriter(dialog.FileName.ToString(), false, Encoding.UTF8))
                            {   
                                stream.WriteLine("ФИО: " + name  + "\nСредний кол-во шагов: " + average + "\nЛуший результат: " + bestres
                                    + "\nХудший результат: " + worstres
                                    + "\nСтатус: " + status
                                    + "\nРанг: " + rank);//запись файл в формате csv
                            }
                        }
                        MessageBox.Show("Запись прошла успешна!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка!" + ex.Message);
                    }
                }
            }
        }

        //functions
        public void GetDataFromDataBase()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Statistics_ WHERE ФИО = @name", sqlConnection);
            sqlCommand.Parameters.AddWithValue("name", NameText);

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();

            if (sqlDataReader.HasRows)
            {
                name = (string)sqlDataReader.GetValue(0);
                bestres = (int)sqlDataReader.GetValue(2);
                worstres = (int)sqlDataReader.GetValue(3);
                status = (string)sqlDataReader.GetValue(4);
                rank = (int)sqlDataReader.GetValue(5);
            }
            sqlDataReader.Close();
            GetAverageDate();
        }

        private void GetAverageDate()
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT СреднееКолВоШагов FROM Statistics_ WHERE ФИО = @name", sqlConnection);
            sqlCommand.Parameters.AddWithValue("name", NameText);
            average = Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        private void UpdateDataGridAndDataTable(string query)
        {
            sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            DataTable = new DataTable();

            sqlDataAdapter.Fill(DataTable);
            DataGrid1.ItemsSource = DataTable.DefaultView;
        }
        private void GoToMainWindow()
        {
            MainWindowAction mainWindow = new MainWindowAction();
            mainWindow.Show();
            this.Close();
        }

        ///обработчики события
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            GoToMainWindow();
        }
        private void Button_Close(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void Button_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Button_Back(object sender, MouseButtonEventArgs e)
        {
            GoToMainWindow();
        }
        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
