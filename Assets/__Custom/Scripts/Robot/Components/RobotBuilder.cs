using Hackcreeper.LD54.Helper;
using Hackcreeper.LD54.LogicEditor.Signals;
using Hackcreeper.LD54.Robot.Data;
using Hackcreeper.LD54.Robot.Enums;
using Hackcreeper.LD54.Robot.Systems;
using Hackcreeper.LD54.Ui.Signals;
using UniDi;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class RobotBuilder : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private RobotBrain robot;

        [SerializeField] private ModuleSo[] availableModules;
        [SerializeField] private LayerMask attachmentAreaLayerMask;

        #endregion

        #region VARIABLES

        private RobotModule _activeModule;
        private Camera _camera;

        [Inject] private readonly RobotLimit _robotLimit;
        [Inject] private readonly SignalBus _signalBus;

        #endregion

        #region LIFECYCLE METHODS

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            robot.GetCoreModule().Place(
                Vector3.zero,
                Vector3.zero,
                Vector3Int.zero,
                robot
            );
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<ModuleSelectedSignal>(OnModuleSelected);
            _signalBus.Subscribe<LogicEditorToggledSignal>(OnLogicEditorToggled);
        }
        
        private void OnDisable()
        {
            _signalBus.Unsubscribe<ModuleSelectedSignal>(OnModuleSelected);
            _signalBus.Unsubscribe<LogicEditorToggledSignal>(OnLogicEditorToggled);
        }

        private void Update()
        {
            if (!_activeModule)
            {
                return;
            }

            var side = _activeModule.GetActiveAttachmentArea()?.GetSide();

            _activeModule.SetErrorState(!_activeModule.CanBePlaced(side ?? AttachmentSide.Back, robot));

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                Destroy(_activeModule.gameObject);
                _activeModule = null;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceActiveModule();
                return;
            }

            MoveActiveModule();
        }

        #endregion

        #region EVENT LISTENERS

        private void OnModuleSelected(ModuleSelectedSignal signal)
        {
            if (_activeModule)
            {
                Destroy(_activeModule.gameObject);
                _activeModule = null;
            }

            var module = Instantiate(signal.Module.prefab);

            _activeModule = module.GetComponent<RobotModule>();
            _activeModule.EnablePlaceholderMode();

            var offset = _activeModule.GetCenter().position - _activeModule.transform.position;
            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            module.transform.position = _camera.ScreenToWorldPoint(mousePosition) - offset;
        }

        private void OnLogicEditorToggled(LogicEditorToggledSignal signal)
        {
            if (!_activeModule)
            {
                return;
            }
            
            Destroy(_activeModule.gameObject);
            _activeModule = null;
        }
        
        #endregion

        #region PUBLIC METHODS

        public ModuleSo[] GetAvailableModules() => availableModules;

        #endregion

        #region PRIVATE METHODS

        private void MoveActiveModule()
        {
            var target = CustomCameraHelper.RayFromMouseToTarget(_camera, attachmentAreaLayerMask);

            if (target)
            {
                _activeModule.LockInPlace(target.GetComponentInParent<AttachmentArea>());
                return;
            }

            _activeModule.EnablePlaceholderMode();
        }

        private void PlaceActiveModule()
        {
            var side = _activeModule.GetActiveAttachmentArea()?.GetSide();

            if (!_activeModule.CanBePlaced(side ?? AttachmentSide.Top, robot))
            {
                return;
            }

            _activeModule.PlaceAtActiveArea();
            _activeModule = null;
        }

        #endregion
    }
}