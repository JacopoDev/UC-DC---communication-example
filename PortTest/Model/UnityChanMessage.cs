namespace PortTest.Model
{
    public struct Message
    {
        public string role;
        public string content;
        public string imageBase64;
    }

    public class ChatTaskContent : UnityChanContent
    {
        public bool? AnswerBack;
        public Message Message;
    }

    public class RewardTaskContent : UnityChanContent
    {
        public int SkillPoints;
        public int? VictoryPoints;
        public string Message;
    }

    public class RelationTaskContent : UnityChanContent
    {
        public int MoodModifier;
        public int? RelationModifier;
    }

    public class ReactionTaskContent : UnityChanContent
    {
        public EExpressedEmotion Emotion;
    }
    
    public class PropTaskContent : UnityChanContent
    {
        public EUnityChanProps Prop;
        public bool Value;
    }

    public class GameRegisterContent : UnityChanContent
    {
        public bool? IsPlay;
        public bool? IsRegister;
        public string GameName;
        public string ExecutablePath;
        public string ImagePath;
        public string Description;
        public bool IsUnityChanBasicComments;
    }

    public class UnityChanContent
    {
        
    }
    
    public class UnityChanMessage
    {
        public EMessageTask Task;
        public UnityChanContent Content;
    }
}
