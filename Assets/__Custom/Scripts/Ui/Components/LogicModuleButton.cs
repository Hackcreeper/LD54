using Hackcreeper.LD54.LogicEditor.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hackcreeper.LD54.Ui.Components
{
    public class LogicModuleButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region EXPOSED FIELDS

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI labelText;

        #endregion

        #region VARIABLES

        private Transform _originalParent;
        private int _originalSiblingIndex;
        private LogicModuleSo _config;

        #endregion

        #region PUBLIC METHODS

        public void Initialize(LogicModuleSo module)
        {
            _config = module;
            labelText.text = module.label;
        }

        public LogicModuleSo GetConfig() => _config;
        
        #endregion
        
        #region OVERRIDDEN METHODS

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();
            
            transform.SetParent(transform.root);
            
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(_originalParent);
            transform.SetSiblingIndex(_originalSiblingIndex);
            canvasGroup.blocksRaycasts = true;
        }

        #endregion
    }
}