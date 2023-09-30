using System;
using System.Collections.Generic;
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

        [SerializeField] private Transform center;

        #endregion

        #region VARIABLES

        private ModuleMode _mode = ModuleMode.Placed;
        private Camera _mainCamera;
        private AttachmentArea _activeAttachmentArea;
        
        private readonly Dictionary<AttachmentSide, RobotModule> _attachedModules = new();
        private readonly Dictionary<AttachmentSide, AttachmentArea> _attachmentAreas = new();

        #endregion

        #region LIFECYCLE METHODS

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_mode != ModuleMode.Placeholder)
            {
                return;
            }

            // Move object to cursor
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            transform.position = _mainCamera.ScreenToWorldPoint(mousePosition);
        }

        #endregion

        #region PUBLIC METHODS

        public void EnablePlaceholderMode()
        {
            _mode = ModuleMode.Placeholder;
        }

        public void LockInPlace(AttachmentArea area)
        {
            var trans = transform;
            var areaTrans = area.transform;
            
            _mode = ModuleMode.StickyPlaceholder;
            trans.position = areaTrans.position;
            trans.rotation = areaTrans.rotation;
            
            _activeAttachmentArea = area;
        }

        public void Place(Vector3 position, Vector3 rotation)
        {
            _mode = ModuleMode.Placed;
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);

            SpawnAttachments();
        }

        public void PlaceAtActiveArea()
        {
            _activeAttachmentArea.GetParentModule().AttachModule(
                _activeAttachmentArea.GetSide(),
                this
            );
        }

        public bool CanBePlaced() => _mode == ModuleMode.StickyPlaceholder;

        private void AttachModule(AttachmentSide side, RobotModule module)
        {
            _attachedModules.Add(side, module);
            module.Place(center.position, GetRotation(side));
            module.transform.SetParent(transform);

            if (!_attachmentAreas.ContainsKey(side))
            {
                return;
            }
            
            Destroy(_attachmentAreas[side].gameObject);
            _attachmentAreas.Remove(side);
        }
        
        #endregion

        #region PRIVATE METHODS

        private void SpawnAttachments()
        {
            if (allowAttachmentTop) SpawnAttachment(AttachmentSide.Top);
            if (allowAttachmentBottom) SpawnAttachment(AttachmentSide.Bottom);
            if (allowAttachmentLeft) SpawnAttachment(AttachmentSide.Left);
            if (allowAttachmentRight) SpawnAttachment(AttachmentSide.Right);
            if (allowAttachmentFront) SpawnAttachment(AttachmentSide.Front);
            if (allowAttachmentBack) SpawnAttachment(AttachmentSide.Back);
        }

        private static Vector3 GetRotation(AttachmentSide side)
        {
            return side switch
            {
                AttachmentSide.Top => Vector3.zero,
                AttachmentSide.Bottom => new Vector3(0, 0, 180),
                AttachmentSide.Left => new Vector3(0, 0, 90),
                AttachmentSide.Right => new Vector3(0, 0, 270),
                AttachmentSide.Front => new Vector3(90, 0, 0),
                AttachmentSide.Back => new Vector3(270, 0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }

        private void SpawnAttachment(AttachmentSide side)
        {
            var area = Instantiate(attachmentAreaPrefab, transform);
            area.transform.position = center.position;
            area.transform.rotation = Quaternion.Euler(GetRotation(side));
            
            var areaComponent = area.GetComponent<AttachmentArea>();
            areaComponent.Initialize(this, side);
            _attachmentAreas.Add(side, areaComponent);
        }

        #endregion
    }
}