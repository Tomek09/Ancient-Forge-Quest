using System;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Managers;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.Utility;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AncientForgeQuest.UI.Inventories
{
    public class InventorySlotView : BindableBehaviour<InventorySlot>
    {
        [Header("Components")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _button;
        [SerializeField] private ItemView _itemView;
        [SerializeField] private TMP_Text _amountText;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Toggle(bool value)
        {
            _canvasGroup.Toggle(value);
        }

        protected override void OnBind()
        {
            if (Model == null)
                return;

            Model.Item.Subscribe(OnItemChange).AddTo(_disposables);
            Model.Amount.Subscribe(OnAmountChange).AddTo(_disposables);
        }
        
        private void OnButtonClick()
        {
            InventoryManager.Instance.OnInventorySlotSelected(this);
        }
        
        private void OnItemChange(ItemModel itemModel)
        {
            _itemView.Bind(itemModel);
        }

        private void OnAmountChange(int amount)
        {
            _amountText.SetText(Model.GetAmountText());
        }
    }
}
