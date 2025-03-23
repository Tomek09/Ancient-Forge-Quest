using System;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Models/Quest")]
    public class QuestModel : Model
    {
        [Header("Components")]
        [SerializeField] private QuestType _questType;
        [SerializeField] private int _intValue;
        [SerializeField] private int _requiredValue;

        [Header("Rewards")]
        [SerializeField] private MachineModel _machineReward;

        public QuestType QuestType
        {
            get => _questType;
        }

        public int IntValue
        {
            get => _intValue;
        }

        public int RequiredValue
        {
            get => _requiredValue;
        }
        
        public MachineModel MachineModelReward
        {
            get => _machineReward;
        }
    }
}
