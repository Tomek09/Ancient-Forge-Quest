using System;
using AncientForgeQuest.Machines;
using AncientForgeQuest.Managers;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using AncientForgeQuest.UI.Inventories;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace AncientForgeQuest.UI.Machines
{
    public class MachineView : BindableBehaviour<MachineInstance>
    {
        [Header("Components")]
        [SerializeField] private MachineModel _modelReference;
        
        [Header("UI Components")]
        [SerializeField] private InventorySlotView[] _inputs;
        [SerializeField] private InventorySlotView _output;
        [SerializeField] private ProgressBar _progressBar;
        [SerializeField] private Button _craftButton;

        private void OnEnable()
        {
            _craftButton.onClick.AddListener(OnCraftButton);
        }
        
        private void OnDisable()
        {
            _craftButton.onClick.RemoveAllListeners();
        }

        private void Start()
        {
            var controller = MachineManager.Instance.Subscribe(_modelReference);
            Bind(controller);
        }

        protected override void OnBind()
        {
            if (Model == null)
                return;

            Model.CraftDuration.Subscribe(OnCraftTimeChange).AddTo(_disposables);
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            var slots = Model.Inventory.Slots;
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i].Bind(slots[i]);
            }
            
            var lastSlot = slots[_inputs.Length];
            _output.Bind(lastSlot);
        }

        private void OnCraftButton()
        {
            Model.CraftRequest();
        }

        private void OnCraftTimeChange(TimeSpan timeSpan)
        {
            if (Model.CurrentCraftDuration == TimeSpan.Zero)
            {
                _progressBar.SetFill(0, true);
                return;
            }

            var percentage = (float)(timeSpan.TotalMilliseconds / Model.CurrentCraftDuration.TotalMilliseconds);
            _progressBar.SetFill(1 - percentage, true);
        }
    }
}
