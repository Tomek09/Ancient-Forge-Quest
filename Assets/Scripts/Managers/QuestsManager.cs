using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Instances;
using AncientForgeQuest.Models;
using AncientForgeQuest.Quests;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class QuestsManager : Singleton<QuestsManager>
    {
        [Header("Components")]
        [SerializeField] private QuestModel[] _startQuests;
        
        private readonly Dictionary<QuestType, HashSet<QuestInstance>> _activeQuests = new Dictionary<QuestType, HashSet<QuestInstance>>();

        protected override void OnAwake()
        {
            foreach (var quest in _startQuests)
            {
                var instance = new QuestInstance(quest);
                var key = quest.QuestType;
                if (!_activeQuests.ContainsKey(key))
                {
                    _activeQuests.Add(key, new HashSet<QuestInstance>());
                }
                
                _activeQuests[key].Add(instance);
            }
        }

        public void Increment(QuestIncrement questIncrement)
        {
            var questType = questIncrement.QuestType;
            foreach (var quest in _activeQuests[questType])
            {
                if (quest.State.CurrentValue == QuestState.Completed)
                    continue;
                
                if (!quest.CanIncrement(questIncrement))
                    continue;
                
                quest.Increment(questIncrement.Value);

                if (quest.State.CurrentValue != QuestState.Completed)
                    continue;
                
                OnQuestComplete(quest);
            }
        }

        private void OnQuestComplete(QuestInstance questIncrement)
        {
            var machineReward = questIncrement.BaseModel.MachineModelReward;
            if (machineReward == null)
                return;

            MachineManager.Instance.Unlock(machineReward);

            Debug.Log(questIncrement.BaseModel.name);
        }
    }
}
