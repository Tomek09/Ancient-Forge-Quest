using System.Text;
using AncientForgeQuest.Instances;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Models;
using AncientForgeQuest.UI.Tooltips;
using UnityEngine;

namespace AncientForgeQuest.UI.Machines
{
    public class MachineRecipeBookController : MonoBehaviour, ITooltipProvider
    {
        [Header("Components")]
        [SerializeField] private MachineView _machineView;
        private TooltipContent _content;

        public TooltipContent GetContent()
        {
            if (_content == null)
            {
                CreateTooltipContent();
            }

            return _content;
        }

        private void CreateTooltipContent()
        {
            var recipes = _machineView.Model.BaseModel.Recipes;
            var builder = new StringBuilder();
            foreach (RecipeModel recipe in recipes)
            {
                builder.Append(GetRecipeString(recipe));
                builder.AppendLine();
            }

            _content = new TooltipContent("Recipe Book", builder.ToString());
        }

        private string GetRecipeString(RecipeModel recipe)
        {
            string text = string.Empty;
            var requestItems = recipe.RequiredItems;
            for (int i = 0; i < requestItems.Length; i++)
            {
                text += GetText(requestItems[i]);
                if (i < requestItems.Length - 1)
                    text += " + ";
            }

            text += $" = {GetText(recipe.ResultItem)}";
            return text;

            string GetText(ItemBag itemBag)
            {
                string amount = itemBag.Amount > 1 ? $" x {itemBag.Amount}x" : string.Empty;
                return $"{itemBag.Item.Name}{amount}";
            }
        }
    }
}
