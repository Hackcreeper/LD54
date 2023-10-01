using System.Collections;
using Hackcreeper.LD54.Robot.Data;
using Hackcreeper.LD54.Robot.Enums;
using Hackcreeper.LD54.Ui.Signals;
using TMPro;
using UniDi;
using UnityEngine;
using UnityEngine.UI;

namespace Hackcreeper.LD54.Ui.Components
{
    public class ModuleButton : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject costsBadge;
        [SerializeField] private TextMeshProUGUI costs;
        [SerializeField] private Button button;

        #endregion

        #region VARIABLES

        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region PUBLIC METHODS

        public void Initialize(ModuleSo module)
        {
            icon.sprite = module.icon;
            label.text = module.label;
            
            costsBadge.SetActive(module.type == ModuleType.Upgrade);
            costs.text = module.costs.ToString();

            StartCoroutine(RoutineRegisterListener(module));
        }

        #endregion

        #region PRIVATE METHODS

        private IEnumerator RoutineRegisterListener(ModuleSo module)
        {
            yield return new WaitForEndOfFrame();
            
            button.onClick.AddListener(() => _signalBus.Fire(new ModuleSelectedSignal(module)));
        }

        #endregion
    }
}