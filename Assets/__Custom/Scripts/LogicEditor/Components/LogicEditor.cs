using Hackcreeper.LD54.LogicEditor.Signals;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.LogicEditor.Components
{
    public class LogicEditor : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private GameObject logicEditorPanel;

        #endregion
        
        #region VARIABLES

        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region LIFECYCLE METHODS

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
    }
}