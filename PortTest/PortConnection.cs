using System;
using System.Net.Sockets;
using System.Text;

namespace PortTest
{
    public static class PortConnection
    {
        public static async void SendJsonAwaitAnswer(string json, string ipAddress, int port)
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
        
        public static async void SendJson(string json, string ipAddress, int port)
        {
            try
            {
                using (TcpClient client = new TcpClient(ipAddress, port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(json);

                    // Send the data to the server
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
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