using System;
using System.Net;
using System.Threading.Tasks;
using PortTest.Model;

namespace PortTest
{
    public static class TaskServices
    {
        public static void Chat(int portValue)
        {
            bool answerBack = AskForAnswerBack();

            Console.WriteLine("Write message to Kohaku:");
            string cmd = Console.ReadLine();

            Task<ApiResponse> sendTask = PortConnection.SendChat(cmd, answerBack, portValue);
            sendTask.Wait();
            WriteResponse(sendTask.Result);
            
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }

        public static void Reward(int portValue)
        {
            bool answerBack = AskForAnswerBack();

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

            Task<ApiResponse> sendTask = PortConnection.SendReward(rewardText, skillReward, victoryReward, answerBack, portValue);
            sendTask.Wait();
            WriteResponse(sendTask.Result);
            
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }

        public static void Relation(int portValue)
        {
            bool answerBack = AskForAnswerBack();

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

            Task<ApiResponse> sendTask = PortConnection.SendRelation(moodModifier, relationModifier, answerBack, portValue);
            sendTask.Wait();
            WriteResponse(sendTask.Result);
            
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }

        public static void Reaction(int portValue)
        {
            bool answerBack = AskForAnswerBack();

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

            Task<ApiResponse> sendTask = PortConnection.SendReaction(emotion, answerBack, portValue);
            sendTask.Wait();

            WriteResponse(sendTask.Result);
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }

        public static void Prop(int portValue)
        {
            bool answerBack = AskForAnswerBack();

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

            Task<ApiResponse>  sendTask = PortConnection.SendSetProp(prop, (bool)valueToSet, answerBack, portValue);
            sendTask.Wait();
            WriteResponse(sendTask.Result);
            
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }

        public static void GameRegister(int portValue)
        {
            bool answerBack = AskForAnswerBack();

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
                Console.WriteLine(
                    "Should Unity-chan comment the screen at random periods of time (2-5 minutes)? (y/n):");
                string valueInput = Console.ReadLine();
                valueInput = valueInput.ToLower();
                if (valueInput == "y") isEnableRandomComments = true;
                if (valueInput == "n") isEnableRandomComments = false;
            }

            Task<ApiResponse> sendTask = PortConnection.SendGameRegister(gameTitle, gameDescription, gameExecutablePath,
                gameImagePath,
                isGameToRegister, isGameToPlay, (bool)isEnableRandomComments, answerBack, portValue);
            sendTask.Wait();
            WriteResponse(sendTask.Result);
            
            Console.WriteLine("Click 'enter' to continue");
            Console.ReadLine();
        }
        
        private static bool AskForAnswerBack()
        {
            while (true)
            {
                Console.WriteLine("Request API answer? (y/n):");
                string valueInput = Console.ReadLine();
                valueInput = valueInput.ToLower();
                if (valueInput == "y") return true;
                if (valueInput == "n") return false;
            }
        }

        private static void WriteResponse(ApiResponse response)
        {
            if (response == null) return;

            if (response.StatusCode != (int)HttpStatusCode.OK)
            {
                Console.WriteLine($"Return code: {response.StatusCode}");
            }
            Console.WriteLine(response.Body);
        }
    }
}