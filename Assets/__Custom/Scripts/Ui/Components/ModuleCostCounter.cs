using Hackcreeper.LD54.Robot.Enums;
using Hackcreeper.LD54.Robot.Signals;
using Hackcreeper.LD54.Robot.Systems;
using TMPro;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.Ui.Components
{
    public class ModuleCostCounter : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private TextMeshProUGUI counterText;

        #endregion
        
        #region VARIABLES

        [Inject] private readonly RobotLimit _robotLimit;
        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region LIFECYCLE METHODS

        private void OnEnable()
        {
            _signalBus.Subscribe<AfterModuleAttachedSignal>(OnAfterModuleAttached);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<AfterModuleAttachedSignal>(OnAfterModuleAttached);
        }

        #endregion
        
        #region EVENT LISTENERS

        private void OnAfterModuleAttached(AfterModuleAttachedSignal signal)
        {
            if (signal.Module.GetConfig().type == ModuleType.Structure)
            {
                return;
            }

            var remaining = _robotLimit.MaxModulePoints - signal.Module.GetRobot().GetTotalModuleCosts();
            
            counterText.text = $"{remaining} module points remaining";
        }
        
        #endregion
    }
}