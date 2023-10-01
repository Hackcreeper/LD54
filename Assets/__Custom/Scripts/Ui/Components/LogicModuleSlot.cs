using System;
using Hackcreeper.LD54.LogicEditor.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hackcreeper.LD54.Ui.Components
{
    public class LogicModuleSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region EXPOSED FIELDS

        [SerializeField] private Image dropArea;
        [SerializeField] private Color dropHoverColor;
        [SerializeField] private Color dropDefaultColor;
        [SerializeField] private GameObject dropInstructionText;
        [SerializeField] private Button deleteButton;
        [SerializeField] private TextMeshProUGUI titleText;

        #endregion

        #region VARIABLES

        private LogicModuleSo _module;
        private GameObject _moduleInner;

        #endregion

        #region LIFECYCLE METHODS

        private void Start()
        {
            deleteButton.onClick.AddListener(OnDelete);
        }

        #endregion

        #region EVENT LISTENERS

        private void OnDelete()
        {
            deleteButton.gameObject.SetActive(false);
            dropInstructionText.SetActive(true);
            Destroy(_moduleInner);

            _module = null;
        }

        #endregion
        
        #region OVERRIDDEN METHODS

        public void OnDrop(PointerEventData eventData)
        {
            var btn = eventData.pointerDrag.GetComponent<LogicModuleButton>();
            if (!btn || _module)
            {
                return;
            }

            dropArea.color = dropDefaultColor;
            dropInstructionText.SetActive(false);

            _module = btn.GetConfig();
            
            var ui = Instantiate(_module.uiPrefab, dropArea.transform);
            ui.GetComponent<AbstractLogicSlot>().Initialize(_module);
            _moduleInner = ui;
            
            deleteButton.gameObject.SetActive(true);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!eventData.dragging || _module)
            {
                return;
            }

            dropArea.color = dropHoverColor;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            dropArea.color = dropDefaultColor;
        }
        
        #endregion

        #region PUBLIC METHODS

        public void Initialize(int index)
        {
            titleText.text = $"Logic slot #{index}";
        }

        #endregion
    }
}