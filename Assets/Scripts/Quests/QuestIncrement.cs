namespace AncientForgeQuest.Quests
{
    public struct QuestIncrement
    {
        public readonly QuestType QuestType;
        public readonly int Value;
        public readonly int IntValue;

        public QuestIncrement(QuestType questType, int value, int intValue)
        {
            QuestType = questType;
            Value = value;
            IntValue = intValue;
        }
    }
}
