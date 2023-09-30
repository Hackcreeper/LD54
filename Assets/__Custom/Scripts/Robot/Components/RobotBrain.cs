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
            _modules.Add(signal.Coordinates, signal.Module);
        }
        
        #endregion
        
        #region PUBLIC METHODS

        public bool HasModuleAt(Vector3Int coords) => _modules.ContainsKey(coords);

        public RobotModule GetCoreModule() => coreModule;

        public int Count(ModuleType type)
        {
            return _modules.Values.Count(module => module.GetConfig().type == type);
        }
        
        #endregion
    }
}