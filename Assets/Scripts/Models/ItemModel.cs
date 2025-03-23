using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Models/Item")]
    public class ItemModel : ScriptableObject
    {
        [Header("Values")]
        [SerializeField] private int _itemID;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _maxSize;
        
        public int ItemID
        {
            get => _itemID;
        }

        public string Name
        {
            get => _name;
        }

        public string Description
        {
            get => _description;
        }

        public Sprite Icon
        {
            get => _icon;
        }

        public int MaxSize
        {
            get => _maxSize;
        }
    }
}
