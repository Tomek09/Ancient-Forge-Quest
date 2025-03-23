using AncientForgeQuest.Inventories;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Models/Recipe")]
    public class RecipeModel : ScriptableObject
    {
        [Header("Values")]
        [SerializeField] private ItemBag[] _requiredItems;
        [SerializeField] private ItemBag[] _resultItems;
        [SerializeField, Tooltip("Enter duration in seconds.")] private int _duration;
        [SerializeField, Range(0, 1)] private float _baseSuccessRate;
        
        public ItemBag[] RequiredItems
        {
            get => _requiredItems;
        }

        public ItemBag[] ResultItems
        {
            get => _resultItems;
        }

        public float Duration
        {
            get => _duration;
        }

        public float BaseSuccessRate
        {
            get => _baseSuccessRate;
        }
    }
}
