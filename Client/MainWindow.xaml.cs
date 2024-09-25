using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Media.Media3D;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        IPAddress ip;
        int port;
        TcpClient client;
        BinaryReader br;
        BinaryWriter bw;

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Car> cars;

        public List<Car> Cars
        {
            get { return cars; }
            set { cars = value; OnPropertyChanged(nameof(cars)); }
        }

        private Car selectedCar;

        public Car SelectedCar
        {
            get { return selectedCar; }
            set { selectedCar = value; OnPropertyChanged(nameof(Cars)); }
        }


        public MainWindow()
        {
            try
            {
                Cars = new List<Car>();
                selectedCar = new Car();
                InitializeComponent();
                ConfigureTCPConnection();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "MAIN");
            }
        }

        void ConfigureTCPConnection()
        {
            ip = IPAddress.Loopback;
            port = 27001;
            client = new TcpClient();
            client.Connect(ip, port);
            var stream = client.GetStream();
            br = new BinaryReader(stream);
            bw = new BinaryWriter(stream);
        }
        void ClientRequest(Command command)
        {

            switch (command.HttpCommand)
            {
                case HttpCommand.GET:

                    var getCars = GetMethod();

                    if (getCars.Count() > 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Cars = getCars;
                            Cars_LV.ItemsSource = Cars;
                        });
                    }
                    else
                        Dispatcher.Invoke(() => { Cars_LV.ItemsSource = new List<Car>(); });

                    break;

                case HttpCommand.POST:
                    PostMethod();
                    break;
                case HttpCommand.PUT:
                    PutMethod();
                    break;
                case HttpCommand.DELETE:
                    DeleteMethod();
                    break;
                default:
                    break;
            }






        }

        List<Car> GetMethod()
        {
            Command getCommand = new Command() { HttpCommand = HttpCommand.GET };
            var SendingCommand = JsonConvert.SerializeObject(getCommand);
            bw.Write(SendingCommand);
            var response = br.ReadString();
            var gettingResult = JsonConvert.DeserializeObject<List<Car>>(response);

            return gettingResult;
        }

        void PostMethod()
        {
            Command postCommand = new Command()
            {
                HttpCommand = HttpCommand.POST,
                Value = new Car() { Marka = "Added car" + Counter.IdCounter + 1, Model = "Added Model" + Counter.IdCounter + 1, Year = 2023 }
            };
            var SendingPostCommand = JsonConvert.SerializeObject(postCommand);
            bw.Write(SendingPostCommand);
        }

        void PutMethod()
        {


            Command PutCommand = new Command()
            {
                HttpCommand = HttpCommand.PUT,
                Value = new Car() { Marka = "Updated Marka by Put Method", Model = "Updated by Put Method", Year = 2012 },
                Index = selectedCar.Id
            };
            var SendingPutCommand = JsonConvert.SerializeObject(PutCommand);
            bw.Write(SendingPutCommand);
            var putMethodResponce = br.ReadString();
        }

        void DeleteMethod()
        {
            Command deleteCommand = new Command() { HttpCommand = HttpCommand.DELETE, Index = selectedCar.Id };
            var sendingDeleteCommand = JsonConvert.SerializeObject(deleteCommand);
            bw.Write(sendingDeleteCommand);
        }

        async private void GetMethod_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Command getCommand = new() { HttpCommand = HttpCommand.GET };
                await Task.Run(() => ClientRequest(getCommand));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private async void Post_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Command postCommand = new() { HttpCommand = HttpCommand.POST };
                await Task.Run(() => { ClientRequest(postCommand); });
                Thread.Sleep(1);
                await View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Put_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                selectedCar = Cars_LV.SelectedItem as Car;
                if (selectedCar != null)
                {

                    Command putCommand = new() { HttpCommand = HttpCommand.PUT };
                    await Task.Run(() => { ClientRequest(putCommand); });
                    Thread.Sleep(1);
                    await View();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Command deleteCommand = new() { HttpCommand = HttpCommand.DELETE };
                selectedCar = Cars_LV.SelectedItem as Car;
                if (selectedCar != null)
                {
                    await Task.Run(() => { ClientRequest(deleteCommand); });
                    Thread.Sleep(1);
                    await View();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        async Task View()
        {
            Command getCommand = new() { HttpCommand = HttpCommand.GET };
            await Task.Run(() => { ClientRequest(getCommand); });
        }
    }
}