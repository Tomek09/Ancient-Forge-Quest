using AncientForgeQuest.Models;
using AncientForgeQuest.UI.DesignSystem;
using UnityEngine;
namespace AncientForgeQuest.Machines
{
    public class MachineController : BindableBehaviour<MachineModel>
    {
        [Header("Components")]
        [SerializeField] private MachineModel _modelReference;

        protected override void OnBind()
        {
            
        }
    }
}
