using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AncientForgeQuest.Models
{
    [CreateAssetMenu(fileName = "New Machine", menuName = "Models/Machine")]
    public class MachineModel : Model
    {
        [Header("Values")]
        [SerializeField] private RecipeModel[] _recipes;
        [SerializeField] private bool _isUnlocked;
        
        public RecipeModel[] Recipes
        {
            get => _recipes;
        }

        public bool IsUnlocked
        {
            get => _isUnlocked;
        }
        
        public int GetInputs()
        {
            return _recipes.Aggregate(0, (current, recipe) => Mathf.Max(current, recipe.RequiredItems.Length));
        }

        public Dictionary<ItemModel, List<RecipeModel>> CatchRecipes()
        {
            var recipesByItem = new Dictionary<ItemModel, List<RecipeModel>>();
            foreach (var recipe in Recipes)
            {
                foreach (var requiredItem in recipe.RequiredItems)
                {
                    if (!recipesByItem.ContainsKey(requiredItem.Item))
                    {
                        recipesByItem[requiredItem.Item] = new List<RecipeModel>();
                    }

                    recipesByItem[requiredItem.Item].Add(recipe);
                }
            }

            return recipesByItem;
        }
    }
}
