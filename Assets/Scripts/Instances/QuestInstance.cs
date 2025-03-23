using AncientForgeQuest.Models;
using AncientForgeQuest.Quests;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Instances
{
    public class QuestInstance : Instance<QuestModel>
    {
        public ReactiveProperty<QuestState> State { get; private set; }
        public ReactiveProperty<int> Progress { get; private set; }

        public QuestInstance(QuestModel baseModel) : base(baseModel)
        {
            State = new ReactiveProperty<QuestState>(QuestState.InProgress);
            Progress = new ReactiveProperty<int>(0);
        }

        public void Increment(int value)
        {
            if (State.CurrentValue != QuestState.InProgress)
                return;
            
            Progress.Value += value;
            if (Progress.CurrentValue < BaseModel.RequiredValue)
                return;
            
            State.Value = QuestState.Completed;
        }

        public bool CanIncrement(QuestIncrement questIncrement)
        {
            return questIncrement.IntValue == BaseModel.IntValue;
        }
    }
}
