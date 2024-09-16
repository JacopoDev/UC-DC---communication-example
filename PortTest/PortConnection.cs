using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortTest.Model;

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

        public static void SendChat(string cmd, int portValue)
        {
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

            Task messenger = Task.Run(() => SendJsonAwaitAnswer(json, "127.0.0.1", portValue));
            messenger.Wait();
        }

        public static void SendReward(string rewardText, int skillReward, int? victoryReward, int portValue)
        {
            // JSON object to send
            var dataReward = new UnityChanMessage()
            {
                Task = EMessageTask.Reward,
                Content = new RewardTaskContent()
                {
                    Message = rewardText,
                    SkillPoints = skillReward,
                    VictoryPoints = victoryReward
                }
            };

            // Serialize object to JSON
            string jsonReward = JsonConvert.SerializeObject(dataReward);

            // Send JSON to the specified IP and port

            Task messengerReward = Task.Run(() => SendJson(jsonReward, "127.0.0.1", portValue));
            messengerReward.Wait();
        }

        public static void SendRelation(int moodModifier, int? relationModifier, int portValue)
        {
            // JSON object to send
            var dataRelation = new UnityChanMessage()
            {
                Task = EMessageTask.Relation,
                Content = new RelationTaskContent()
                {
                    MoodModifier = moodModifier,
                    RelationModifier = relationModifier
                }
            };

            // Serialize object to JSON
            string jsonRelation = JsonConvert.SerializeObject(dataRelation);

            // Send JSON to the specified IP and port

            Task messengerRelation = Task.Run(() => SendJson(jsonRelation, "127.0.0.1", portValue));
            messengerRelation.Wait();
        }

        public static void SendReaction(EExpressedEmotion emotion, int portValue)
        {
            // JSON object to send
            var dataReaction = new UnityChanMessage()
            {
                Task = EMessageTask.Reaction,
                Content = new ReactionTaskContent()
                {
                    Emotion = emotion
                }
            };

            // Serialize object to JSON
            string jsonReaction = JsonConvert.SerializeObject(dataReaction);

            // Send JSON to the specified IP and port

            Task messengerReaction = Task.Run(() => SendJson(jsonReaction, "127.0.0.1", portValue));
            messengerReaction.Wait();
        }

        public static void SendSetProp(EUnityChanProps prop, bool? valueToSet, int portValue)
        {
            // JSON object to send
            var dataProp = new UnityChanMessage()
            {
                Task = EMessageTask.Prop,
                Content = new PropTaskContent()
                {
                    Prop = prop,
                    Value = (bool)valueToSet
                }
            };

            // Serialize object to JSON
            string jsonProp = JsonConvert.SerializeObject(dataProp);

            // Send JSON to the specified IP and port

            Task messengerProp = Task.Run(() => SendJson(jsonProp, "127.0.0.1", portValue));
            messengerProp.Wait();
        }

        public static void SendGameRegister(string gameTitle, string gameDescription, string gameExecutablePath,
            string gameImagePath, bool? isGameToRegister, bool? isGameToPlay, bool? isEnableRandomComments, int portValue)
        {
            // JSON object to send
            var dataGameRegister = new UnityChanMessage()
            {
                Task = EMessageTask.GameRegister,
                Content = new GameRegisterContent()
                {
                    GameName = gameTitle,
                    Description = gameDescription,
                    ExecutablePath = gameExecutablePath,
                    ImagePath = gameImagePath,
                    IsRegister = isGameToRegister,
                    IsPlay = isGameToPlay,
                    IsUnityChanBasicComments = (bool)isEnableRandomComments
                }
            };

            // Serialize object to JSON
            string jsonGameRegister = JsonConvert.SerializeObject(dataGameRegister);

            // Send JSON to the specified IP and port

            Task messengerGameRegister = Task.Run(() => SendJson(jsonGameRegister, "127.0.0.1", portValue));
            messengerGameRegister.Wait();
        }
    }
}