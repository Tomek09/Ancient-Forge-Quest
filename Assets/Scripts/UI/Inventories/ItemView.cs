using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AncientForgeQuest.UI.Inventories
{
    public class ItemView : BindableBehaviour<ItemModel>
    {
        [Header("Components")]
        [SerializeField] private Image _icon;

        protected override void OnBind()
        {
            if (Model == null)
            {
                ToggleIcon(false);
                return;
            }

            ToggleIcon(true);
            _icon.sprite = Model.Icon;
        }

        private void ToggleIcon(bool value)
        {
            _icon.enabled = value;
        }
    }
}
