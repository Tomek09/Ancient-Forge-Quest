using System;
using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Instances;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Machines
{
    public class MachineInstance : Instance<MachineModel>
    {
        public MachineInventory Inventory { get; private set; }
        private readonly Dictionary<ItemModel, List<RecipeModel>> _recipesByItem = new Dictionary<ItemModel, List<RecipeModel>>();

        public ReactiveProperty<TimeSpan> CraftTime = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<bool> InUnlocked = new ReactiveProperty<bool>();

        private RecipeModel _currentRecipe;
        private bool _autoCraft = true;

        public MachineInstance(MachineModel model) : base(model)
        {
            Inventory = new MachineInventory(model.GetInputs() + 1);
            InUnlocked.Value = model.IsUnlocked;
            CacheRecipes();
        }

        private void CacheRecipes()
        {
            var recipes = BaseModel.Recipes;
            foreach (var recipe in recipes)
            {
                foreach (var requiredItem in recipe.RequiredItems)
                {
                    if (!_recipesByItem.ContainsKey(requiredItem.Item))
                    {
                        _recipesByItem[requiredItem.Item] = new List<RecipeModel>();
                    }

                    _recipesByItem[requiredItem.Item].Add(recipe);
                }
            }
        }

        public void CraftRequest()
        {
            if (!InUnlocked.CurrentValue)
                return;
            
            if (_currentRecipe)
                return;

            if (!IsCraftable(out var recipe))
                return;

            _currentRecipe = recipe;
            OnCraftStarted();
        }

        public void Tick(float deltaTime)
        {
            if (_currentRecipe == null)
                return;

            CraftTime.Value = CraftTime.CurrentValue.DecreaseDeltaTime(deltaTime);
        }

        public bool HasRecipe()
        {
            return _currentRecipe != null;
        }

        public bool IsCraftingCompleted()
        {
            return CraftTime.CurrentValue.IsCompleted();
        }

        private void OnCraftStarted()
        {
            CraftTime.Value = TimeSpan.FromSeconds(_currentRecipe.Duration);
            ConsumeInput(_currentRecipe);
        }

        public ItemModel OnCraftingCompleted()
        {
            var item = AddResult();

            if (_autoCraft && IsCraftable(_currentRecipe))
            {
                OnCraftStarted();
                return item;
            }

            _currentRecipe = null;
            return item;
        }

        private void ConsumeInput(RecipeModel recipeModel)
        {
            var inputSlots = Inventory.InputSlots;
            var requiredItems = recipeModel.RequiredItems;
            foreach (var slot in inputSlots)
            {
                if (slot.IsEmpty())
                    continue;

                var bag = requiredItems.FirstOrDefault(x => slot.HasItem(x.Item));
                slot.Inventory.Remove(slot, bag.Amount);
            }
        }

        private ItemModel AddResult()
        {
            var output = Inventory.OutputSlot;
            var result = _currentRecipe.ResultItem;
            if (!output.HasItem(result.Item))
            {
                output.Bind(result.Item, result.Amount);
            }
            else
            {
                output.Inventory.Add(output, result.Amount);
            }

            return result.Item;
        }

        private bool IsCraftable(out RecipeModel recipe)
        {
            recipe = null;
            return TryGetRecipe(out recipe) && IsCraftable(recipe);
        }

        private bool IsCraftable(RecipeModel recipe)
        {
            var output = Inventory.OutputSlot;
            var resultItem = recipe.ResultItem;

            if (!output.IsEmpty() && !output.HasItem(resultItem.Item))
                return false;

            if (!output.IsEmpty() && output.Amount.CurrentValue + resultItem.Amount > resultItem.Item.MaxSize)
                return false;

            return IsRecipeValid(recipe);
        }

        private bool TryGetRecipe(out RecipeModel recipeModel)
        {
            recipeModel = null;
            var inputSlots = Inventory.InputSlots;
            var possibleRecipes = new HashSet<RecipeModel>();

            foreach (var slot in inputSlots)
            {
                if (slot.IsEmpty())
                    return false;

                if (!_recipesByItem.TryGetValue(slot.Item.CurrentValue, out var recipes))
                    continue;

                foreach (var recipe in recipes)
                {
                    possibleRecipes.Add(recipe);
                }
            }

            var currentItems = new Dictionary<ItemModel, int>();
            foreach (var slot in inputSlots)
            {
                currentItems.TryAdd(slot.Item.CurrentValue, 0);
                currentItems[slot.Item.CurrentValue] += slot.Amount.CurrentValue;
            }

            recipeModel = possibleRecipes.FirstOrDefault(recipe => IsRecipeValid(recipe, currentItems));
            return recipeModel != null;
        }

        private bool IsRecipeValid(RecipeModel recipeModel)
        {
            var currentItems = new Dictionary<ItemModel, int>();
            var inputSlots = Inventory.InputSlots;
            foreach (var slot in inputSlots)
            {
                if (slot.IsEmpty())
                    continue;

                currentItems.TryAdd(slot.Item.CurrentValue, 0);
                currentItems[slot.Item.CurrentValue] += slot.Amount.CurrentValue;
            }

            return IsRecipeValid(recipeModel, currentItems);
        }

        private bool IsRecipeValid(RecipeModel recipeModel, Dictionary<ItemModel, int> currentItems)
        {
            foreach (var required in recipeModel.RequiredItems)
            {
                if (!currentItems.TryGetValue(required.Item, out var amount) || amount < required.Amount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
