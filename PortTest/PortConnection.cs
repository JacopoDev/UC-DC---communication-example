using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortTest.Model;

namespace PortTest
{
    public static class PortConnection
    {
        private static async Task<ApiResponse> SendJsonAwaitAnswer(string json, string ipAddress, int port)
        {
            ApiResponse apiResponse = null;
            
            using (TcpClient client = new TcpClient(ipAddress, port))
            using (NetworkStream stream = client.GetStream())
            {
                byte[] bytesToSend = Encoding.UTF8.GetBytes(json);

                stream.Write(bytesToSend, 0, bytesToSend.Length);
                    
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                }
                    
                client.Close();
            }
            
            return apiResponse;
        }
        
        private static void SendJson(string json, string ipAddress, int port)
        {
            using (TcpClient client = new TcpClient(ipAddress, port))
            using (NetworkStream stream = client.GetStream())
            {
                byte[] bytesToSend = Encoding.UTF8.GetBytes(json);

                stream.Write(bytesToSend, 0, bytesToSend.Length);
                client.Close();
            }
        }

        private static async Task<ApiResponse> SendData(bool requestAnswer, int portValue, UnityChanMessage data)
        {
            string json = JsonConvert.SerializeObject(data);
            if (requestAnswer)
            {
                ApiResponse response = await SendJsonAwaitAnswer(json, IPAddress.Loopback.ToString(), portValue);
                return response;
            }
            else
            {
                SendJson(json, IPAddress.Loopback.ToString(), portValue);
            }
            
            return null;
        }

        public static async Task<ApiResponse> SendChat(string cmd, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
            {
                Task = EMessageTask.Chat,
                Content = new ChatTaskContent()
                {
                    Message = new Message()
                    {
                        role = "system",
                        content = cmd,
                        imageBase64 = null
                    }
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }

        public static async Task<ApiResponse> SendReward(string rewardText, int skillReward, int? victoryReward, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
            {
                Task = EMessageTask.Reward,
                Content = new RewardTaskContent()
                {
                    Message = rewardText,
                    SkillPoints = skillReward,
                    VictoryPoints = victoryReward
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }

        public static async Task<ApiResponse> SendRelation(int moodModifier, int? relationModifier, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
            {
                Task = EMessageTask.Relation,
                Content = new RelationTaskContent()
                {
                    MoodModifier = moodModifier,
                    RelationModifier = relationModifier
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }

        public static async Task<ApiResponse> SendReaction(EExpressedEmotion emotion, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
            {
                Task = EMessageTask.Reaction,
                Content = new ReactionTaskContent()
                {
                    Emotion = emotion
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }

        public static async Task<ApiResponse> SendSetProp(EUnityChanProps prop, bool valueToSet, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
            {
                Task = EMessageTask.Prop,
                Content = new PropTaskContent()
                {
                    Prop = prop,
                    Value = valueToSet
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }

        public static async Task<ApiResponse> SendGameRegister(string gameTitle, string gameDescription, string gameExecutablePath,
            string gameImagePath, bool? isGameToRegister, bool? isGameToPlay, bool isEnableRandomComments, bool requestAnswer, int portValue)
        {
            var data = new UnityChanMessage()
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
                    IsUnityChanBasicComments = isEnableRandomComments
                }
            };

            return await SendData(requestAnswer, portValue, data);
        }
    }
}