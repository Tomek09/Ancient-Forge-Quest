using System.Collections.Generic;
using AncientForgeQuest.Machines;
using AncientForgeQuest.Models;
using AncientForgeQuest.Quests;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class MachineManager : Singleton<MachineManager>
    {
        private readonly Dictionary<MachineModel, MachineInstance> _instances = new Dictionary<MachineModel, MachineInstance>();
        
        public MachineInstance Subscribe(MachineModel model)
        {
            if (_instances.ContainsKey(model))
            {
                Debug.LogError("Machine already subscribed.");
                return null;
            }

            var controller = new MachineInstance(model);
            _instances.Add(model, controller);
            return controller;
        }

        public void Unlock(MachineModel model)
        {
            if (!_instances.TryGetValue(model, out var instance))
            {
                Debug.LogError($"Can't find instance: {model.name}.");
                return;
            }
            
            instance.InUnlocked.Value = true;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var machine in _instances.Values)
            {
                if (!machine.HasRecipe())
                    continue;
                
                machine.Tick(deltaTime);

                if (!machine.IsCraftingCompleted())
                    continue;

                var resultItem = machine.OnCraftingCompleted();
                QuestsManager.Instance.Increment(new QuestIncrement(QuestType.Craft, 1, resultItem.ItemID));
            }
        }
    }
}
