using Hackcreeper.LD54.LogicEditor.Data;
using UnityEngine;

namespace Hackcreeper.LD54.Ui.Components
{
    public abstract class AbstractLogicSlot : MonoBehaviour
    {
        #region ABSTRACT METHODS

        public abstract void Initialize(LogicModuleSo module);

        #endregion
    }
}