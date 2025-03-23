using System;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Bonus Item", menuName = "Models/Items/Bonus")]
    public class BonusItem : ItemModel
    {
        [Header("Bonuses")]
        [SerializeField] private BonusType _bonusType;
        [SerializeField] private float _value;

        public BonusType BonusType
        {
            get => _bonusType;
        }

        public float Value
        {
            get => _value;
        }

        public override string GetDescription()
        {
            return _bonusType switch
            {
                BonusType.IncreasesCraftChance => $"Increases success rate by {Mathf.RoundToInt(_value * 100f)} percentage points",
                BonusType.ReducesCraftTime => $"Reduces crafting time by {Mathf.RoundToInt(_value)} seconds.",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
