using System;
using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Machines
{
    public class MachineController
    {
        public MachineInventory Inventory { get; private set; }
        private readonly MachineModel _model;
        private readonly Dictionary<ItemModel, List<RecipeModel>> _recipesByItem = new Dictionary<ItemModel, List<RecipeModel>>();

        public ReactiveProperty<TimeSpan> CraftTime = new ReactiveProperty<TimeSpan>();
        private RecipeModel _currentRecipe;
        private bool _autoCraft = true;

        public MachineController(MachineModel model)
        {
            _model = model;
            Inventory = new MachineInventory(model.GetInputs() + 1);
            CacheRecipes();
        }

        private void CacheRecipes()
        {
            var recipes = _model.Recipes;
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
            if (!CraftTime.CurrentValue.IsCompleted())
                return;

            OnCraftingCompleted();
        }

        private void OnCraftStarted()
        {
            CraftTime.Value = TimeSpan.FromSeconds(_currentRecipe.Duration);
            ConsumeInput(_currentRecipe);
        }

        private void OnCraftingCompleted()
        {
            AddResult();

            if (_autoCraft && IsCraftable(_currentRecipe))
            {
                OnCraftStarted();
                return;
            }

            _currentRecipe = null;
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

        private void AddResult()
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
