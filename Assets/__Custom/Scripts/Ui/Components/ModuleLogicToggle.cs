using Hackcreeper.LD54.LogicEditor.Signals;
using UniDi;
using UnityEngine;
using UnityEngine.UI;

namespace Hackcreeper.LD54.Ui.Components
{
    public class ModuleLogicToggle : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private Button modulesButton;
        [SerializeField] private Button logicButton;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;
        
        #endregion

        #region VARIABLES

        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region LIFECYCLE METHODS

        private void Start()
        {
            modulesButton.onClick.AddListener(() =>
            {
                _signalBus.Fire(new LogicEditorToggledSignal(false));
                modulesButton.image.color = activeColor;
                logicButton.image.color = inactiveColor;
            });
            logicButton.onClick.AddListener(() =>
            {
                _signalBus.Fire(new LogicEditorToggledSignal(true));
                modulesButton.image.color = inactiveColor;
                logicButton.image.color = activeColor;
            });
        }

        #endregion
    }
}