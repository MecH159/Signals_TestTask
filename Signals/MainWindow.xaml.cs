using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
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

namespace Signals
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        private ChartValues<double> _sinValues;

        //Создаём коллекцию для передачи точек в график LiveChart через тег SignalValues
        public ChartValues<double> SignalValues
        {
            get { return _sinValues; }
            set
            {
                _sinValues = value;
                OnPropertyChanged("SignalValues");
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();
            _sinValues = new ChartValues<double>();
            DataContext = this;
        }

        //PropertyChangedEventHandler нужен для LiveChart, для заполнения коллекции ChartValues по тегу из xaml
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //Метод для наполнения коллекции точками с значениями
        public ChartValues<double> GetGraph(ChartValues<double> values, SignalBase signal, string signalType, double amplitude, double frequency, int pointsCount)
        {
            signal.SignalGeneratorBase(amplitude, frequency, pointsCount);
            double[] points = signal.Generate();
            for (int i = 0; i < points.Length; i++)
            {
                values.Add(points[i]);
            }
            return values;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SignalValues.Clear();
                string signalType = ((ComboBoxItem)cmbSignalType.SelectedItem).Content.ToString();
                double amplitude = Convert.ToDouble(txtAmplitude.Text);
                double frequency = Convert.ToDouble(txtFrequency.Text);
                int pointsCount = Convert.ToInt32(txtPointsCount.Text);
                if (signalType == "Синус")
                {
                    Sinusoid sinus = new Sinusoid();
                    GetGraph(SignalValues, sinus, signalType, amplitude, frequency, pointsCount);

                }
                else if (signalType == "Меандр")
                {
                    Meandr meandr = new Meandr();
                    GetGraph(SignalValues, meandr, signalType, amplitude, frequency, pointsCount);
                }
                else if (signalType == "Пилообразный")
                {
                    SawTooth saw = new SawTooth();
                    GetGraph(SignalValues, saw, signalType, amplitude, frequency, pointsCount);
                }
                else if (signalType == "Треугольный")
                {
                    Triangle tri = new Triangle();
                    GetGraph(SignalValues, tri, signalType, amplitude, frequency, pointsCount);
                }

                double sum = 0;
                int zeroCrossing = 0;

                // Считаем сумму точек
                foreach (var point in SignalValues)
                {
                    sum += point;
                }

                // Считаем пересечения нуля
                for (int i = 1; i < SignalValues.Count; i++)
                {
                    if (SignalValues[i - 1] * SignalValues[i] < 0)
                    {
                        zeroCrossing++;
                    }
                }
                txtMaxValue.Content = SignalValues.Max().ToString();
                txtMinValue.Content = SignalValues.Min().ToString();
                txtSumValue.Content = sum.ToString();
                txtZeroCount.Content = zeroCrossing.ToString();
                btnSave.IsEnabled = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Сохраение графика в БД через System.Data.SQLite
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=SignalBD.db"))
            {
                try
                {
                    connection.Open();

                    double amplitude = Convert.ToDouble(txtAmplitude.Text);
                    double frequency = Convert.ToDouble(txtFrequency.Text);
                    int ID_Type = cmbSignalType.SelectedIndex + 1;

                    string sqlQuery = @"INSERT INTO Signal (Amplitude, Frequency, ID_SignalType, DateInsert) 
                               VALUES (@amplitude, @frequency, @id_type, @date)";

                    using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@amplitude", amplitude);
                        command.Parameters.AddWithValue("@frequency", frequency);
                        command.Parameters.AddWithValue("@id_type", ID_Type);
                        command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("График успешно сохранён в базу данных!");
                            btnSave.IsEnabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Ошибка: данные не были сохранены");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
                }
            }
        }
        
        
    
    }
}

