using System;
using System.Collections.Generic;
using AncientForgeQuest.Machines;
using AncientForgeQuest.Models;
using AncientForgeQuest.Utility;
using UnityEngine;

namespace AncientForgeQuest.Managers
{
    public class MachineManager : Singleton<MachineManager>
    {
        private readonly Dictionary<MachineModel, MachineController> _machines = new Dictionary<MachineModel, MachineController>();
        
        public MachineController Subscribe(MachineModel model)
        {
            if (_machines.ContainsKey(model))
            {
                Debug.LogError("Machine already subscribed");
                return null;
            }

            var controller = new MachineController(model);
            _machines.Add(model, controller);
            return controller;
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            
            foreach (var machine in _machines.Values)
            {
                machine.Tick(deltaTime);
            }
        }
    }
}
