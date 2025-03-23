using AncientForgeQuest.Inventories;
using AncientForgeQuest.Inventories.Interfaces;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    [RequireComponent(typeof(InventorySlotView))]
    public class PickupController : MonoBehaviour, IPickable
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        private InventorySlotView _slotView;

        public InventorySlot Model => _slotView.Model;

        private void Awake()
        {
            _slotView = GetComponent<InventorySlotView>();
        }

        public void OnPickupStart()
        {
            _canvasGroup.SetAlpha(false);
        }

        public void OnPickupEnd()
        {
            _canvasGroup.SetAlpha(true);
        }

        public bool IsPickable()
        {
            return !Model.IsEmpty();
        }
    }
}
