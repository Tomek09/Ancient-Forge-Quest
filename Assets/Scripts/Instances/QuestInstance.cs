using AncientForgeQuest.Managers;
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
        
        public float GetCompletionProgress()
        {
            return Progress.CurrentValue / (float)BaseModel.RequiredValue;
        }

        public string GetDescription()
        {
            if (BaseModel.QuestType == QuestType.Craft && ItemManager.Instance.TryGetItem(BaseModel.IntValue, out var item))
            {
                return $"Craft {BaseModel.RequiredValue} {item.Name}";
            }

            return string.Empty;
        }
    }
}
