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
            EMessageTask task = EMessageTask.Chat;
            int inputTask = -1;
            int portValue = 0;
            bool portCorrect = false;
            
            while (!portCorrect)
            {
                Console.Clear();
                Console.WriteLine("Setting connection to localhost 127.0.0.1...");
                Console.WriteLine("Set port:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out portValue))
                {
                    portCorrect = true;
                }
            }
            
            while (inputTask < 0 || inputTask > (int)EMessageTask.GameRegister)
            {
                Console.Clear();
                Console.WriteLine("Select task to send:");
                Console.WriteLine("1 - send chat message");
                Console.WriteLine("2 - give reward");
                Console.WriteLine("3 - modify relation");
                Console.WriteLine("4 - set reaction");
                Console.WriteLine("5 - set prop");
                Console.WriteLine("6 - register / play game");
                Console.WriteLine("7 - exit");
                
                string input = Console.ReadLine();
                if (int.TryParse(input, out inputTask))
                {
                    if (inputTask == 7)
                    {
                        return;
                    }
                    
                    if (inputTask > 0 && inputTask <= ((int)EMessageTask.GameRegister) + 1)
                    {
                        task = (EMessageTask)(inputTask - 1);
                    }
                    else
                    {
                        inputTask = -1;
                        continue;
                    }
                }
                else
                {
                    inputTask = -1;
                    continue;
                }
                
                switch (task)
                {
                    case EMessageTask.Chat:
                        Console.WriteLine("Write message to Kohaku:");
                        string cmd = Console.ReadLine();
                    
                        // JSON object to send
                        var data = new UnityChanMessage()
                        {
                            Task = task,
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
                        
                        Task messenger = Task.Run(() => PortConnection.SendJsonAwaitAnswer(json, "127.0.0.1", portValue));
                        messenger.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        break;
                    
                    case EMessageTask.Reward:
                        
                        Console.WriteLine("Write reward message:");
                        string rewardText = Console.ReadLine();

                        int skillReward = -1;
                        while (skillReward == -1)
                        {
                            Console.WriteLine("Write skill points reward amount:");
                            string skillRewardText = Console.ReadLine();
                            int.TryParse(skillRewardText, out skillReward);
                        }

                        int? victoryReward = -1;
                        while (victoryReward == -1)
                        {
                            Console.WriteLine("Write victory points reward amount:");
                            string victoryRewardText = Console.ReadLine();
                            if (int.TryParse(victoryRewardText, out int value))
                            {
                                if (value == 0)
                                {
                                    victoryReward = null;
                                }
                                else
                                {
                                    victoryReward = value;
                                }
                            }
                        }
                    
                        // JSON object to send
                        var dataReward = new UnityChanMessage()
                        {
                            Task = task,
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
                        
                        Task messengerReward = Task.Run(() => PortConnection.SendJson(jsonReward, "127.0.0.1", portValue));
                        messengerReward.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        
                        break;
                    case EMessageTask.Relation:
                        
                        int moodModifier = -1;
                        while (moodModifier == -1)
                        {
                            Console.WriteLine("Write mood modifier amount:");
                            string moodModifierText = Console.ReadLine();
                            int.TryParse(moodModifierText, out moodModifier);
                        }

                        int? relationModifier = -1;
                        while (relationModifier == -1)
                        {
                            Console.WriteLine("Write relation modifier amount:");
                            string relationModifierText = Console.ReadLine();
                            if (int.TryParse(relationModifierText, out int value))
                            {
                                if (value == 0)
                                {
                                    relationModifier = null;
                                }
                                else
                                {
                                    relationModifier = value;
                                }
                            }
                        }
                    
                        // JSON object to send
                        var dataRelation = new UnityChanMessage()
                        {
                            Task = task,
                            Content = new RelationTaskContent()
                            {
                                MoodModifier = moodModifier,
                                RelationModifier = relationModifier
                            }
                        };
                
                        // Serialize object to JSON
                        string jsonRelation = JsonConvert.SerializeObject(dataRelation);
                
                        // Send JSON to the specified IP and port
                        
                        Task messengerRelation = Task.Run(() => PortConnection.SendJson(jsonRelation, "127.0.0.1", portValue));
                        messengerRelation.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        
                        break;
                    case EMessageTask.Reaction:

                        EExpressedEmotion emotion = EExpressedEmotion.None;
                        int reactionSelection = -1;
                        while (reactionSelection == -1)
                        {
                            Console.WriteLine("Select reaction to play:");
                            Console.WriteLine("0 - none");
                            Console.WriteLine("1 - happy");
                            Console.WriteLine("2 - angry");
                            Console.WriteLine("3 - joking");
                            Console.WriteLine("4 - embarrassed");
                            Console.WriteLine("5 - flirty");
                            Console.WriteLine("6 - sad");
                            Console.WriteLine("7 - surprised");
                            Console.WriteLine("8 - closedeyes");
                            string reactionSelectionText = Console.ReadLine();
                            if (int.TryParse(reactionSelectionText, out int value))
                            {
                                if (value >= 0 && value <= (int)EExpressedEmotion.ClosedEyes)
                                {
                                    emotion = (EExpressedEmotion)value;
                                    reactionSelection = value;
                                }
                            }
                        }
                    
                        // JSON object to send
                        var dataReaction = new UnityChanMessage()
                        {
                            Task = task,
                            Content = new ReactionTaskContent()
                            {
                                Emotion = emotion
                            }
                        };
                
                        // Serialize object to JSON
                        string jsonReaction = JsonConvert.SerializeObject(dataReaction);
                
                        // Send JSON to the specified IP and port
                        
                        Task messengerReaction = Task.Run(() => PortConnection.SendJson(jsonReaction, "127.0.0.1", portValue));
                        messengerReaction.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        break;
                    
                    case EMessageTask.Prop:
                        
                        EUnityChanProps prop = EUnityChanProps.GamePad;
                        int propSelection = -1;
                        while (propSelection == -1)
                        {
                            Console.WriteLine("Select prop:");
                            Console.WriteLine("0 - Gamepad");
                            Console.WriteLine("1 - Headphone");
                            Console.WriteLine("2 - Phone");
                            string propSelectionText = Console.ReadLine();
                            if (int.TryParse(propSelectionText, out int value))
                            {
                                if (value >= 0 && value <= (int)EUnityChanProps.Phone)
                                {
                                    prop = (EUnityChanProps)value;
                                    propSelection = value;
                                }
                            }
                        }

                        bool? valueToSet = null;

                        while (valueToSet == null)
                        {
                            Console.WriteLine("Enable? (y/n):");
                            string valueInput = Console.ReadLine();
                            valueInput = valueInput.ToLower();
                            if (valueInput == "y") valueToSet = true;
                            if (valueInput == "n") valueToSet = false;
                        }
                    
                        // JSON object to send
                        var dataProp = new UnityChanMessage()
                        {
                            Task = task,
                            Content = new PropTaskContent()
                            {
                                Prop = prop,
                                Value = (bool)valueToSet
                            }
                        };
                
                        // Serialize object to JSON
                        string jsonProp = JsonConvert.SerializeObject(dataProp);
                
                        // Send JSON to the specified IP and port
                        
                        Task messengerProp = Task.Run(() => PortConnection.SendJson(jsonProp, "127.0.0.1", portValue));
                        messengerProp.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        break;
                    
                    case EMessageTask.GameRegister:

                        bool? isGameToRegister = null;
                        bool? isGameToPlay = null;
                        bool? isEnableRandomComments = null;

                        while (isGameToRegister == null)
                        {
                            Console.WriteLine("Should game be registered? (y/n):");
                            string valueInput = Console.ReadLine();
                            valueInput = valueInput.ToLower();
                            if (valueInput == "y") isGameToRegister = true;
                            if (valueInput == "n") isGameToRegister = false;
                        }

                        while (isGameToPlay == null)
                        {
                            Console.WriteLine("Should game be played immediately? (y/n):");
                            string valueInput = Console.ReadLine();
                            valueInput = valueInput.ToLower();
                            if (valueInput == "y") isGameToPlay = true;
                            if (valueInput == "n") isGameToPlay = false;
                        }
                        
                        Console.WriteLine("Type in game title:");
                        string gameTitle = Console.ReadLine();

                        Console.WriteLine("Describe the game (200 letters max):");
                        string gameDescription = Console.ReadLine();

                        Console.WriteLine("Paste game executable path:");
                        string gameExecutablePath = Console.ReadLine();

                        Console.WriteLine("Paste game preview image path:");
                        string gameImagePath = Console.ReadLine();
                        
                        while (isEnableRandomComments == null)
                        {
                            Console.WriteLine("Should Unity-chan comment the screen at random periods of time (2-5 minutes)? (y/n):");
                            string valueInput = Console.ReadLine();
                            valueInput = valueInput.ToLower();
                            if (valueInput == "y") isEnableRandomComments = true;
                            if (valueInput == "n") isEnableRandomComments = false;
                        }
                    
                        // JSON object to send
                        var dataGameRegister = new UnityChanMessage()
                        {
                            Task = task,
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
                        
                        Task messengerGameRegister = Task.Run(() => PortConnection.SendJson(jsonGameRegister, "127.0.0.1", portValue));
                        messengerGameRegister.Wait();
                        Console.WriteLine("Click 'enter' to continue");
                        Console.ReadLine();
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();

                }
                
                inputTask = -1;
            }
        }
    }
}