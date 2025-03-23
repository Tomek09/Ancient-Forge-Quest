using System;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Managers;
using R3;
using UnityEngine;

namespace AncientForgeQuest.UI.Inventories
{
    public class DraggableSlotController : MonoBehaviour, IDisposable
    {
        [SerializeField] private DraggableSlotInventoryView _draggableSlotView;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        private void Start()
        {
            GameInventoryManager.Instance.SelectedSlot.Subscribe(OnInventorySlotChanged).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }

        private void OnInventorySlotChanged(InventorySlot slot)
        {
            _draggableSlotView.Bind(slot);
        }
    }
}
