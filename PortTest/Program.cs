using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortTest.Model;

namespace PortTest
{

    internal partial class Program
    {
        static void Main(string[] args)
        {
            MainMenuLoop();
        }

        private static void MainMenuLoop()
        {
            EMessageTask task = EMessageTask.Chat;
            int inputTask = -1;
            int portValue = 0;
            bool? answerBack = null;

            portValue = AskPortValue();

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

                inputTask = AskTaskToSend(ref task);
                
                if (inputTask == -1) continue;
                if (inputTask == 7) return;

                switch (task)
                {
                    case EMessageTask.Chat:
                        TaskServices.Chat(portValue);
                        break;
                    
                    case EMessageTask.Reward:
                        TaskServices.Reward(portValue);
                        break;
                    
                    case EMessageTask.Relation:
                        TaskServices.Relation(portValue);
                        break;
                    
                    case EMessageTask.Reaction:
                        TaskServices.Reaction(portValue);
                        break;
                    
                    case EMessageTask.Prop:
                        TaskServices.Prop(portValue);
                        break;

                    case EMessageTask.GameRegister:
                        TaskServices.GameRegister(portValue);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                inputTask = -1;
            }
        }

        private static int AskTaskToSend(ref EMessageTask task)
        {
            int inputTask;
            string input = Console.ReadLine();
            if (int.TryParse(input, out inputTask))
            {
                if (inputTask == 7)
                {
                    return inputTask;
                }

                if (inputTask > 0 && inputTask <= ((int)EMessageTask.GameRegister) + 1)
                {
                    task = (EMessageTask)(inputTask - 1);
                }
                else
                {
                    inputTask = -1;
                    return inputTask;
                }
            }
            else
            {
                inputTask = -1;
                return inputTask;
            }

            return inputTask;
        }

        private static int AskPortValue()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Setting connection to localhost 127.0.0.1...");
                Console.WriteLine("Set port:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int portValue))
                {
                    return portValue;
                }
            }
        }
    }
}