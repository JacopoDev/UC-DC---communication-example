using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortTest.Model;

namespace PortTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cmd = "s";
            string port = "s";
            int portValue = 0;
            bool portCorrect = false;
            
            Console.WriteLine("Setting connection to localhost 127.0.0.1...");
            while (!portCorrect)
            {
                Console.WriteLine("Set port:");
                port = Console.ReadLine();
                if (int.TryParse(port, out portValue))
                {
                    portCorrect = true;
                }
            }
            
            while (cmd != "q")
            {
                Console.WriteLine("Write message to Kohaku:");
                cmd = Console.ReadLine();
                if (cmd == "q")
                {
                    return;
                }
                
                // JSON object to send
                var data = new UnityChanMessage()
                {
                    Task = EMessageTask.Chat,
                    Content = new ChatTaskContent()
                    {
                        AnswerBack = true,
                        Message = new Message()
                        {
                            role = "system",
                            content = cmd,
                            imageBase64 = null
                        }
                    }
                };

                // Serialize object to JSON
                string json = JsonConvert.SerializeObject(data);

                // Send JSON to the specified IP and port
                SendJson(json, "127.0.0.1", portValue);
            }
        }

        static async void SendJson(string json, string ipAddress, int port)
        {
            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(json);

                    // Send the data to the server
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                    
                    // Wait for the server's response
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Received from server: " + response);
                    }
                    else
                    {
                        Console.WriteLine("Server closed the connection or no data received.");
                    }
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}