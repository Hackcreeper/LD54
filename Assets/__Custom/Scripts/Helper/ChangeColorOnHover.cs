using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hackcreeper.LD54.Helper
{
    [RequireComponent(typeof(Image))]
    public class ChangeColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region EXPOSED FIELDS

        [SerializeField] private Color hoverColor = Color.white;

        #endregion
        
        #region VARIABLES

        private Image _image;
        private Color _originalColor;

        #endregion
        
        #region LIFECYCLE METHODS

        private void Awake()
        {
            _image = GetComponent<Image>();
            _originalColor = _image.color;
        }

        #endregion

        #region OVERRIDDEN METHODS

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _originalColor;
        }

        #endregion
    }
}