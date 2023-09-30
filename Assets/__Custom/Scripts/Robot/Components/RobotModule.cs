using Hackcreeper.LD54.Robot.Data;
using Hackcreeper.LD54.Robot.Enums;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class RobotModule : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [Header("General")] [SerializeField] private ModuleSo configuration;

        [Header("Attachments")] [SerializeField]
        private bool allowAttachmentTop;

        [SerializeField] private bool allowAttachmentBottom;
        [SerializeField] private bool allowAttachmentLeft;
        [SerializeField] private bool allowAttachmentRight;
        [SerializeField] private bool allowAttachmentFront;
        [SerializeField] private bool allowAttachmentBack;

        [Header("References")] [SerializeField]
        private GameObject attachmentAreaPrefab;

        #endregion

        #region VARIABLES

        private ModuleMode _mode = ModuleMode.Placed;

        #endregion

        #region LIFECYCLE METHODS
        
        private void Update()
        {
            if (_mode != ModuleMode.Placeholder)
            {
                return;
            }

            // Move object to cursor
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        #endregion

        #region PUBLIC METHODS

        public void EnablePlaceholderMode()
        {
            _mode = ModuleMode.Placeholder;
        }

        public void LockInPlace(Vector3 position)
        {
            _mode = ModuleMode.StickyPlaceholder;
            transform.position = position;
        }

        public void Place(Vector3 position)
        {
            _mode = ModuleMode.Placed;
            transform.position = position;
            
            SpawnAttachments();
        }

        #endregion

        #region PRIVATE METHODS

        private void SpawnAttachments()
        {
            if (allowAttachmentTop) SpawnAttachment(Vector3.zero);
            if (allowAttachmentBottom) SpawnAttachment(new Vector3(0, 0, 180));
            if (allowAttachmentLeft) SpawnAttachment(new Vector3(0, 0, 90));
            if (allowAttachmentRight) SpawnAttachment(new Vector3(0, 0, 270));
            if (allowAttachmentFront) SpawnAttachment(new Vector3(90, 0, 0));
            if (allowAttachmentBack) SpawnAttachment(new Vector3(270, 0, 0));
        }

        private void SpawnAttachment(Vector3 rotation)
        {
            Instantiate(attachmentAreaPrefab, transform).transform.Rotate(rotation);
        }
        
        #endregion
    }
}