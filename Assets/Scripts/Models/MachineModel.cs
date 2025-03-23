using System.Linq;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Machine", menuName = "Models/Machine")]
    public class MachineModel : ScriptableObject
    {
        [Header("Values")]
        [SerializeField] private RecipeModel[] _recipes;
        [SerializeField] private bool _requirements;

        public int GetInputs()
        {
            int total = _recipes.Aggregate(0, (current, recipe) => Mathf.Max(current, recipe.RequiredItems.Length));
            return total;
        }
        
        public RecipeModel[] Recipes
        {
            get => _recipes;
        }
    }
}
