using System;
using System.Collections.Generic;
using System.Linq;
using AncientForgeQuest.Instances;
using AncientForgeQuest.Inventories;
using AncientForgeQuest.Managers;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace AncientForgeQuest.Machines
{
    public class MachineInstance : Instance<MachineModel>, IDisposable
    {
        public readonly ReactiveProperty<TimeSpan> CraftDuration = new ReactiveProperty<TimeSpan>();
        public readonly ReactiveProperty<bool> InUnlocked = new ReactiveProperty<bool>();
        public readonly ReactiveProperty<RecipeModel> CurrentRecipe = new ReactiveProperty<RecipeModel>();
        public MachineInventory Inventory { get; private set; }
        public TimeSpan CurrentCraftDuration = TimeSpan.Zero;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Dictionary<ItemModel, List<RecipeModel>> _recipesByItem;
        private RecipeModel _previousRecipe;

        public bool IsActive { get; private set; }

        public MachineInstance(MachineModel model) : base(model)
        {
            InUnlocked.Value = model.IsUnlocked;
            Inventory = new MachineInventory(model.GetInputs() + 1);
            _recipesByItem = model.CatchRecipes();
            var slots = Inventory.Slots;
            foreach (var inputSlot in slots)
            {
                inputSlot.OnInventorySlotChanged.Subscribe(_ => OnInputSlotChanged()).AddTo(_disposables);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }

        public void CraftRequest()
        {
            if (!InUnlocked.CurrentValue)
                return;

            if (IsActive)
                return;

            if (CurrentRecipe.CurrentValue == null)
                return;

            OnCraftStarted();
        }

        public void Tick(float deltaTime)
        {
            if (!IsActive)
                return;

            CraftDuration.Value = CraftDuration.CurrentValue.DecreaseDeltaTime(deltaTime);
        }

        public bool IsCraftingCompleted()
        {
            return CraftDuration.CurrentValue.IsCompleted();
        }

        private void OnCraftStarted()
        {
            IsActive = true;
            SetTimer();
            ConsumeInput();
        }

        public bool OnCraftingCompleted(out ItemModel resultItem)
        {
            resultItem = null;

            var successRate = GetSuccessRate();
            if (successRate.IsSuccess())
            {
                resultItem = AddResult();
            }

            ResetTimer();
            CurrentRecipe.Value = GetRecipe();
            IsActive = false;

            return resultItem != null;
        }

        private void ConsumeInput()
        {
            var inputSlots = Inventory.InputSlots;
            var requiredItems = CurrentRecipe.CurrentValue.RequiredItems;
            foreach (var slot in inputSlots)
            {
                if (slot.IsEmpty())
                    continue;

                var bag = requiredItems.FirstOrDefault(x => slot.HasItem(x.Item));
                slot.Inventory.Remove(slot, bag.Amount);
            }
        }

        private void ResetTimer()
        {
            CurrentCraftDuration = TimeSpan.Zero;
            CraftDuration.Value = TimeSpan.Zero;
        }

        private void SetTimer()
        {
            float duration = CurrentRecipe.Value.Duration;
            if (BonusesManager.Instance.TryGetBonus(BonusType.ReducesCraftTime, out var bonusValue))
            {
                duration -= bonusValue;
            }
            var finalDuration = TimeSpan.FromSeconds(duration);

            CurrentCraftDuration = finalDuration;
            CraftDuration.Value = finalDuration;
        }

        private void OnInputSlotChanged()
        {
            if (IsActive)
                return;

            CurrentRecipe.Value = GetRecipe();
        }

        private ItemModel AddResult()
        {
            var output = Inventory.OutputSlot;
            var result = CurrentRecipe.CurrentValue.ResultItem;
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


        public float GetSuccessRate()
        {
            var successRate = CurrentRecipe.CurrentValue.BaseSuccessRate;
            if (!BonusesManager.Instance.TryGetBonus(BonusType.IncreasesCraftChance, out var bonusValue))
                return successRate;
            
            successRate += bonusValue / 100f;
            successRate = Mathf.Clamp01(successRate);

            return successRate;
        }


        private RecipeModel GetRecipe()
        {
            var currentItems = GetCurrentItems();
            var possibleRecipes = GetPossibleRecipes(currentItems);

            var recipe = GetValidRecipe(currentItems, possibleRecipes);

            if (recipe == null)
                return null;

            var output = Inventory.OutputSlot;
            var resultItem = recipe.ResultItem;

            if (!output.IsEmpty() && !output.HasItem(resultItem.Item))
                return null;

            if (!output.IsEmpty() && output.Amount.CurrentValue + resultItem.Amount > resultItem.Item.MaxSize)
                return null;

            return recipe;
        }

        private Dictionary<ItemModel, int> GetCurrentItems()
        {
            var currentItems = new Dictionary<ItemModel, int>();
            foreach (var slot in Inventory.InputSlots)
            {
                if (slot.IsEmpty())
                    continue;

                var item = slot.Item.CurrentValue;
                currentItems.TryAdd(item, 0);

                currentItems[item] += slot.Amount.CurrentValue;
            }

            return currentItems;
        }

        private List<RecipeModel> GetPossibleRecipes(Dictionary<ItemModel, int> currentItems)
        {
            var possibleRecipes = new List<RecipeModel>();

            foreach (var slot in Inventory.InputSlots)
            {
                if (slot.IsEmpty())
                    continue;

                if (!_recipesByItem.TryGetValue(slot.Item.CurrentValue, out var recipes))
                    continue;

                possibleRecipes.AddRange(recipes);
            }

            return possibleRecipes;
        }

        private RecipeModel GetValidRecipe(Dictionary<ItemModel, int> currentItems, List<RecipeModel> possibleRecipes)
        {
            foreach (var recipe in possibleRecipes)
            {
                bool isValid = true;

                foreach (var required in recipe.RequiredItems)
                {
                    if (currentItems.TryGetValue(required.Item, out var amount) && amount >= required.Amount)
                        continue;

                    isValid = false;
                    break;
                }

                if (isValid)
                    return recipe;
            }

            return null;
        }
    }
}
