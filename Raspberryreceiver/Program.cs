using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raspberryreceiver.model;

namespace BerthAppRaspberrySensorReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            double pressure;
            double temperature;
            double humidity;

            int number = 0;

            //Creates a UdpClient for reading incoming data.

            UdpClient udpReceiver = new UdpClient();

            // This IPEndPoint will allow you to read datagrams sent from any ip-source on port 6969

            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 6969);

            Console.WriteLine("Receiver is blocked");

            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpReceiver.Receive(ref RemoteIpEndPoint);

                    string receivedData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine("Sender: " + receivedData.ToString());

                    string[] textLines = receivedData.Split('\n');

                    for (int index = 0; index < textLines.Length; index++)
                        Console.Write(textLines[index]);

                    string[] list1 = textLines[0].Split(':');
                    string text1 = list1[1];
                    string[] list2 = textLines[1].Split(':');
                    string text2 = list2[1];
                    string[] list3 = textLines[2].Split(':');
                    string text3 = list3[1];

                    humidity = Double.Parse(text1);
                    pressure = Double.Parse(text2);
                    temperature = Double.Parse(text3);

                    Console.WriteLine("Long: " + humidity);
                    Console.WriteLine("Lat: " + pressure);
                    Console.WriteLine("BpSystolic: " + temperature);

                    Record record = new Record(pressure, temperature, humidity);

                    PostRecordAsync(record);

                    //Console.ReadLine(); //for reading the data slowly
                    Thread.Sleep(1000);

                    number++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static async Task PostRecordAsync(Record record)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://berthapibeta20181025031131.azurewebsites.net");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.PostAsJsonAsync("api/records", record);
            response.EnsureSuccessStatusCode();

            Console.Write("Este es el status actual: " + response);
        }
    }
}
