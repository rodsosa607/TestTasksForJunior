using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using System.Data.SqlClient;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;

namespace AnalysisCountsSteps.MVVM.View
{
    public partial class ChartDiagram : UserControl
    {
        private SqlConnection sqlConnection = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataTable DataTable = null;

        public List<int> ListSteps;

        public Func<double, string> Formatter { get; set; }
        public ChartValues<ObservableValue> Values { get; set; }
        public Brush DangerBrush { get; set; }
        public CartesianMapper<ObservableValue> Mapper { get; set; }

        public ChartDiagram()
        {
            InitializeComponent();

            sqlConnection = new SqlConnection(@"Data Source=DESKTOP-0RUJG5Q\SQLEXPRESS;Initial Catalog=AnalysisCountSteps;Integrated Security=True");
            sqlConnection.Open();

            sqlDataAdapter = new SqlDataAdapter("SELECT * FROM [Users]", sqlConnection);
            DataTable = new DataTable();

            sqlDataAdapter.Fill(DataTable);
        }

        //functions
        public void LoadChart()//метод,который загружает диаграмму
        {
            Values = new ChartValues<ObservableValue>()
            {
                     /*!!!КОМЕЕНТАРИЙ
                    Для того, чтобы не создавать 30 точек, как написано ниже, можно было бы создать следующий цикл
                    for (int i = 0; i < ListSteps.Count - 1; i++) 
                        new ObservableValue(ListSteps[i]);
                    и в цикле создавать каждый раз новую точку т.е. i-ая точка, 
                    но тогда в данном случаем будет ошибка.
                    Поэтому мне пришлось создавать таким образом точки(
                    */
                    new ObservableValue(ListSteps[0]),
                    new ObservableValue(ListSteps[1]),
                    new ObservableValue(ListSteps[2]),
                    new ObservableValue(ListSteps[3]),
                    new ObservableValue(ListSteps[4]),
                    new ObservableValue(ListSteps[5]),
                    new ObservableValue(ListSteps[6]),
                    new ObservableValue(ListSteps[7]),
                    new ObservableValue(ListSteps[8]),
                    new ObservableValue(ListSteps[9]),
                    new ObservableValue(ListSteps[10]),
                    new ObservableValue(ListSteps[11]),
                    new ObservableValue(ListSteps[12]),
                    new ObservableValue(ListSteps[13]),
                    new ObservableValue(ListSteps[14]),
                    new ObservableValue(ListSteps[15]),
                    new ObservableValue(ListSteps[16]),
                    new ObservableValue(ListSteps[17]),
                    new ObservableValue(ListSteps[18]),
                    new ObservableValue(ListSteps[19]),
                    new ObservableValue(ListSteps[20]),
                    new ObservableValue(ListSteps[21]),
                    new ObservableValue(ListSteps[22]),
                    new ObservableValue(ListSteps[23]),
                    new ObservableValue(ListSteps[24]),
                    new ObservableValue(ListSteps[25]),
                    new ObservableValue(ListSteps[26]),
                    new ObservableValue(ListSteps[27]),
                    new ObservableValue(ListSteps[28]),
                    new ObservableValue(ListSteps[29])
            };

            Mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 200 ? DangerBrush : null)
                .Stroke(item => item.Value > 200 ? DangerBrush : null);

            Formatter = x => x + " ms";
            DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));
            DataContext = this;
        }

        public void FindStatisticForUser()//метод,который записывает в лист шаги опред пользователя
        {
            ListSteps = new List<int>();

            SqlCommand sqlCommand = new SqlCommand("SELECT Steps FROM Users WHERE User_ = @User", sqlConnection);
            sqlCommand.Parameters.AddWithValue("User", MainWindowAction.NameText);
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    int steps = sqlDataReader.GetInt32(0);
                    ListSteps.Add(steps);//сохраняем в list значения из sql
                }
            }
        }

        //обработчик события
        private void UpdateDataOnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowAction.NameText == String.Empty) MessageBox.Show("Ошибка!Вы не ввели Фамилию и Имя пользователя или не нажали enter, для которого хотите просмотреть статистику!");
            else
            {
                FindStatisticForUser();
                LoadChart();
            }
        }
    }
}
