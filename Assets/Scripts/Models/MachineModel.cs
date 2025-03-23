using UnityEngine;
namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Machine", menuName = "Models/Machine")]
    public class MachineModel : ScriptableObject
    {
        [Header("Values")]
        [SerializeField] private RecipeModel[] _recipes;
        [SerializeField] private bool _requirements;
    }
}
