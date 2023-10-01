using Hackcreeper.LD54.LogicEditor.Data;
using TMPro;
using UnityEngine;

namespace Hackcreeper.LD54.Ui.Components
{
    public class StaticLogicSlot : AbstractLogicSlot
    {
        #region EXPOSED FIELDS

        [SerializeField] private TextMeshProUGUI labelText;

        #endregion
        
        #region OVERRIDDEN METHODS

        public override void Initialize(LogicModuleSo module)
        {
            labelText.text = module.label;
        }

        #endregion
    }
}