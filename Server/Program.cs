
using Newtonsoft.Json;
using NP05HomeworkServer;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
var ip = IPAddress.Loopback;
var port = 27001;
var server = new TcpListener(ip, port);
server.Start();

var client = server.AcceptTcpClient();
var stream = client.GetStream();
var bw = new BinaryWriter(stream);
var br = new BinaryReader(stream);

List<Car> cars = new List<Car>()
{
    new Car()
    {
        Marka="Test Marka",
        Model="Test Model",
        Year=2022
    }
};

while (true)
{
    var input = br.ReadString();
    var command = JsonConvert.DeserializeObject<Command>(input);
    if (command != null)
        switch (command.HttpCommand)
        {
            case HttpCommand.GET:
                var returningGetMethodResponce = JsonConvert.SerializeObject(cars);
                bw.Write(returningGetMethodResponce);
                break;

            case HttpCommand.POST:
                cars.Add(command.Value);
                break;

            case HttpCommand.PUT:
                var PutCar = cars.FirstOrDefault(c => c.Id == command.Index);
                PutCar.Marka = command.Value.Marka;
                PutCar.Model=command.Value.Model;
                var returnPutMethodResponce = JsonConvert.SerializeObject(PutCar);
                bw.Write(returnPutMethodResponce);
                break;

            case HttpCommand.DELETE:
                var findCar = cars.FirstOrDefault(c => c.Id == command.Index);
                if (findCar != null)
                {
                    cars.Remove(findCar);
                }
                break;
            default:
                break;
        }
}