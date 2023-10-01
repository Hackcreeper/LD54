using Hackcreeper.LD54.LogicEditor.Data;
using Hackcreeper.LD54.LogicEditor.Signals;
using Hackcreeper.LD54.Robot.Systems;
using Hackcreeper.LD54.Ui.Components;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.LogicEditor.Components
{
    public class LogicEditor : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private LogicModuleSo[] availableLogicModules;

        [Header("References")] [SerializeField]
        private GameObject logicEditorPanel;

        [SerializeField] private RectTransform moduleParent;
        [SerializeField] private GameObject moduleButtonPrefab;

        [SerializeField] private RectTransform logicSlotParent;
        [SerializeField] private GameObject logicSlotPrefab;

        #endregion

        #region VARIABLES

        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly RobotLimit _robotLimit;

        #endregion

        #region LIFECYCLE METHODS

        private void Start()
        {
            SpawnModuleButtons();
            SpawnLogicSlots();
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<LogicEditorToggledSignal>(OnLogicEditorToggled);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LogicEditorToggledSignal>(OnLogicEditorToggled);
        }

        #endregion

        #region EVENT LISTENERS

        private void OnLogicEditorToggled(LogicEditorToggledSignal signal)
        {
            logicEditorPanel.SetActive(signal.Open);
        }

        #endregion

        #region PRIVATE METHODS

        private void SpawnModuleButtons()
        {
            foreach (var module in availableLogicModules)
            {
                Instantiate(moduleButtonPrefab, moduleParent).GetComponent<LogicModuleButton>().Initialize(module);
            }
        }
        
        private void SpawnLogicSlots()
        {
            for (var i = 0; i < _robotLimit.MaxLogicModules; i++)
            {
                Instantiate(logicSlotPrefab, logicSlotParent).GetComponent<LogicModuleSlot>().Initialize(i+1);
            }
        }

        #endregion
    }
}