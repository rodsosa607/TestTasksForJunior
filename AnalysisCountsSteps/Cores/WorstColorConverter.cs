using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Globalization;

namespace AnalysisCountsSteps.Cores
{
    class WorstColorConverter : MainWindowAction, IValueConverter
    {
        List<double> ListAverageCouts;
        List<double> ListTheWorstCouts;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value <= 19000 /*(int)ReturnElements(ListAverageCouts, ListTheWorstCouts) -- ошибка! */ ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.White);
            /*!!!КОМЕЕНТАРИЙ
            Данный участок кода отвечает за раскраску строчек в DataGrid.
            Для того, чтобы раскрасить конкретных пользователей, необходимо было получить два столбца из таблицы,
            я получил стобцы СреднееКолВоШагов и ХудшийРезультат и сохранил их в лист для последующей работы.
            ReturnElements() который принимает два листа, определяет коллекцию элементов, которые должны быть закрашены.
            Но, мне необходимо было возвращать из функции в цикле несколько значений, т.е коллекцию, для этого я использовал 
            IEnumerable<object> и yield return для возврата значения. 
            Но метод Convert() возвращает только один объект(кисть в данном случае), и я не смог преобразовать IEnumerable<object> в int 
            из-за чего соответственно метод Convert() раскрашивают строки в DataGrid по одному значению
            */
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        private IEnumerable<object> ReturnElements(List<double> list1, List<double> list2)
        {
            LoadDatesAboutSteps();
            for (int i = -1; i < list1.Count; i++)
            {
                for (int j = i; j < i; j++)
                {
                    double percent = list1[i] * 20 / 100;
                    double num = list1[i] - percent;
                    if (list2[j] < num)
                        yield return num;
                }
            }
        }

        private void LoadDatesAboutSteps()
        {
            ListAverageCouts = new List<double>();
            ListTheWorstCouts = new List<double>();

            SqlCommand sqlCommand = new SqlCommand("SELECT СреднееКолВоШагов FROM Statistics_", sqlConnection);
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read()) ListAverageCouts.Add(sqlDataReader.GetDouble(0));//сохраняем в list значения из sql
            }
            SqlCommand sqlCommand2 = new SqlCommand("SELECT ХудшийРезультат FROM Statistics_", sqlConnection);
            using (SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader())
            {
                while (sqlDataReader.Read()) ListTheWorstCouts.Add(sqlDataReader.GetInt32(0));//сохраняем в list значения из sql
            }
        }
    }
}
