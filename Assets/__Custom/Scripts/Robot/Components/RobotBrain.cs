using System.Collections.Generic;
using System.Linq;
using Hackcreeper.LD54.Robot.Enums;
using Hackcreeper.LD54.Robot.Signals;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class RobotBrain : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private RobotModule coreModule;

        #endregion

        #region VARIABLES

        private readonly HashSet<RobotModule> _uniqueModules = new();
        private readonly Dictionary<Vector3Int, RobotModule> _modules = new();

        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region LIFECYCLE METHODS

        private void OnEnable()
        {
            _signalBus.Subscribe<ModuleAttachedSignal>(OnModuleAttached);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<ModuleAttachedSignal>(OnModuleAttached);
        }

        #endregion

        #region EVENT LISTENERS

        private void OnModuleAttached(ModuleAttachedSignal signal)
        {
            _uniqueModules.Add(signal.Module);

            var module = signal.Module;
            var coordinates = signal.Coordinates;

            for (var x = 0; x < module.GetGridSize().x; x++)
            {
                for (var y = 0; y < module.GetGridSize().y; y++)
                {
                    for (var z = 0; z < module.GetGridSize().z; z++)
                    {
                        _modules.Add(coordinates + new Vector3Int(x, y, z), module);
                    }
                }   
            }
        }

        #endregion

        #region PUBLIC METHODS

        public bool HasModuleAt(Vector3Int coords) => _modules.ContainsKey(coords);

        public RobotModule GetCoreModule() => coreModule;

        public int Count(ModuleType type) => _uniqueModules.Count(module => module.GetConfig().type == type);

        public int GetTotalModuleCosts() => _uniqueModules.Sum(module => module.GetConfig().costs);

        #endregion
    }
}