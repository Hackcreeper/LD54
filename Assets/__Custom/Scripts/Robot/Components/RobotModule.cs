using System;
using System.Collections.Generic;
using System.Linq;
using Hackcreeper.LD54.Robot.Data;
using Hackcreeper.LD54.Robot.Enums;
using Hackcreeper.LD54.Robot.Signals;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class RobotModule : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [Header("Config")] [SerializeField] private ModuleSo config;
        [SerializeField] private SideScaleStruct[] scaleRules;

        [Header("Attachments")] [SerializeField]
        private bool allowAttachmentTop;

        [SerializeField] private bool allowAttachmentBottom;
        [SerializeField] private bool allowAttachmentLeft;
        [SerializeField] private bool allowAttachmentRight;
        [SerializeField] private bool allowAttachmentFront;
        [SerializeField] private bool allowAttachmentBack;

        [Header("Attachments Rules")] [SerializeField]
        private bool allowAttachedBottom;

        [SerializeField] private bool allowAttachedLeft;
        [SerializeField] private bool allowAttachedRight;
        [SerializeField] private bool allowAttachedFront;
        [SerializeField] private bool allowAttachedBack;

        [Header("References")] [SerializeField]
        private GameObject attachmentAreaPrefab;

        [SerializeField] private Transform center;

        [SerializeField] private RobotBrain robot;

        [Header("Materials")] [SerializeField] private Material errorMaterial;

        [SerializeField] private MeshRenderer[] meshRenderers;

        #endregion

        #region VARIABLES

        private ModuleMode _mode = ModuleMode.Placed;
        private Camera _mainCamera;
        private AttachmentArea _activeAttachmentArea;
        private Vector3Int _gridPosition = Vector3Int.zero;
        private bool _errorEnabled;

        private readonly Dictionary<AttachmentSide, AttachmentArea> _attachmentAreas = new();
        private readonly List<Tuple<MeshRenderer, int, Material>> _originalMaterials = new();

        [Inject] private readonly SignalBus _signalBus;

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
            var offset = center.transform.position - transform.position;

            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            transform.position = _mainCamera.ScreenToWorldPoint(mousePosition) - offset;
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ModuleAttachedSignal>(OnModulePlaced);
        }

        #endregion

        #region EVENT LISTENERS

        private void OnModulePlaced(ModuleAttachedSignal signal)
        {
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(-1, 0, 0), AttachmentSide.Left);
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(1, 0, 0), AttachmentSide.Right);
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(0, 0, 1), AttachmentSide.Front);
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(0, 0, -1), AttachmentSide.Back);
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(0, 1, 0), AttachmentSide.Top);
            RemoveIfCoordinatesMatch(signal.Coordinates, new Vector3Int(0, -1, 0), AttachmentSide.Bottom);
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

            foreach (var rule in scaleRules.Where(rule => rule.side == area.GetSide()))
            {
                trans.localScale = rule.scale;
            }
        }

        public void Place(Vector3 position, Vector3 rotation, Vector3Int gridPos, RobotBrain brain)
        {
            TurnOffError();
            
            robot = brain;

            _mode = ModuleMode.Placed;
            _gridPosition = gridPos;

            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);

            _signalBus.Fire(new ModuleAttachedSignal(this, gridPos));

            SpawnAttachments();

            _signalBus.Subscribe<ModuleAttachedSignal>(OnModulePlaced);
        }

        public void PlaceAtActiveArea()
        {
            _activeAttachmentArea.GetParentModule().AttachModule(
                _activeAttachmentArea.GetSide(),
                this
            );
        }

        public bool CanBePlaced(AttachmentSide side)
        {
            if (_mode != ModuleMode.StickyPlaceholder)
            {
                return false;
            }

            return (side == AttachmentSide.Back && allowAttachedBack)
                   || (side == AttachmentSide.Bottom && allowAttachedBottom)
                   || (side == AttachmentSide.Front && allowAttachedFront)
                   || (side == AttachmentSide.Top && allowAttachmentTop)
                   || (side == AttachmentSide.Left && allowAttachedLeft)
                   || (side == AttachmentSide.Right && allowAttachedRight);
        }

        public ModuleSo GetConfig() => config;

        public RobotBrain GetRobot() => robot;

        public void SetErrorState(bool error)
        {
            if (error)
            {
                TurnOnError();
                return;
            }
            
            TurnOffError();
        }

        public Transform GetCenter() => center;

        public AttachmentArea GetActiveAttachmentArea() => _activeAttachmentArea;

        #endregion

        #region PRIVATE METHODS

        private void AttachModule(AttachmentSide side, RobotModule module)
        {
            var newGridPos = _gridPosition + side switch
            {
                AttachmentSide.Top => new Vector3Int(0, 1, 0),
                AttachmentSide.Bottom => new Vector3Int(0, -1, 0),
                AttachmentSide.Left => new Vector3Int(-1, 0, 0),
                AttachmentSide.Right => new Vector3Int(1, 0, 0),
                AttachmentSide.Front => new Vector3Int(0, 0, 1),
                AttachmentSide.Back => new Vector3Int(0, 0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };

            module.Place(center.position, GetRotation(side), newGridPos, robot);
            module.transform.SetParent(transform);

            DestroyAttachmentArea(side);
        }

        private void DestroyAttachmentArea(AttachmentSide side)
        {
            if (!_attachmentAreas.ContainsKey(side))
            {
                return;
            }

            Destroy(_attachmentAreas[side].gameObject);
            _attachmentAreas.Remove(side);
        }

        private void SpawnAttachments()
        {
            if (allowAttachmentTop && !robot.HasModuleAt(_gridPosition + new Vector3Int(0, 1, 0)))
            {
                SpawnAttachment(AttachmentSide.Top);
            }

            if (allowAttachmentBottom && !robot.HasModuleAt(_gridPosition + new Vector3Int(0, -1, 0)))
            {
                SpawnAttachment(AttachmentSide.Bottom);
            }

            if (allowAttachmentLeft && !robot.HasModuleAt(_gridPosition + new Vector3Int(-1, 0, 0)))
            {
                SpawnAttachment(AttachmentSide.Left);
            }

            if (allowAttachmentRight && !robot.HasModuleAt(_gridPosition + new Vector3Int(1, 0, 0)))
            {
                SpawnAttachment(AttachmentSide.Right);
            }

            if (allowAttachmentFront && !robot.HasModuleAt(_gridPosition + new Vector3Int(0, 0, 1)))
            {
                SpawnAttachment(AttachmentSide.Front);
            }

            if (allowAttachmentBack && !robot.HasModuleAt(_gridPosition + new Vector3Int(0, 0, -1)))
            {
                SpawnAttachment(AttachmentSide.Back);
            }
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

        private void RemoveIfCoordinatesMatch(Vector3Int coords, Vector3Int offset, AttachmentSide side)
        {
            if (coords != _gridPosition + offset)
            {
                return;
            }

            DestroyAttachmentArea(side);
        }

        private void TurnOnError()
        {
            if (_errorEnabled)
            {
                return;
            }

            _errorEnabled = true;
            _originalMaterials.Clear();

            Dictionary<MeshRenderer, List<Material>> materials = new();
            
            foreach (var meshRenderer in meshRenderers)
            {
                for (var i = 0; i < meshRenderer.materials.Length; i++)
                {
                    _originalMaterials.Add(new Tuple<MeshRenderer, int, Material>(
                        meshRenderer,
                        i,
                        meshRenderer.materials[i]
                    ));

                    materials.TryAdd(meshRenderer, new List<Material>());
                    materials[meshRenderer].Add(errorMaterial);
                }
                meshRenderer.SetMaterials(materials[meshRenderer]);
            }
        }

        private void TurnOffError()
        {
            if (!_errorEnabled)
            {
                return;
            }

            _errorEnabled = false;
            
            Dictionary<MeshRenderer, List<Material>> materials = new();
            
            foreach (var original in _originalMaterials)
            {
                materials.TryAdd(original.Item1, new List<Material>());
                materials[original.Item1].Add(original.Item3);
            }

            foreach (var material in materials)
            {
                material.Key.SetMaterials(material.Value);
            }
            
            _originalMaterials.Clear();
        }

        #endregion
    }
}