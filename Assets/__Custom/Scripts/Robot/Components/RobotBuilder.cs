using Hackcreeper.LD54.Helper;
using Hackcreeper.LD54.Robot.Data;
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

        #endregion

        #region LIFECYCLE METHODS

        private void Start()
        {
            robot.GetCoreModule().Place(
                Vector3.zero,
                Vector3.zero,
                Vector3Int.zero,
                robot
            );
        }

        private void Update()
        {
            if (_activeModule)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceActiveModule();
                    return;
                }

                MoveActiveModule();
                return;
            }

            if (!Input.GetKeyDown(KeyCode.A))
            {
                return;
            }

            var randomModule = availableModules[UnityEngine.Random.Range(0, availableModules.Length)];
            var module = Instantiate(randomModule.prefab);
            module.GetComponent<RobotModule>().EnablePlaceholderMode();

            _activeModule = module.GetComponent<RobotModule>();
        }

        #endregion
        
        #region PRIVATE METHODS

        private void MoveActiveModule()
        {
            var target = CustomCameraHelper.RayFromMouseToTarget(Camera.main, attachmentAreaLayerMask);

            if (target)
            {
                _activeModule.LockInPlace(target.GetComponentInParent<AttachmentArea>());
                return;
            }

            _activeModule.EnablePlaceholderMode();
        }

        private void PlaceActiveModule()
        {
            if (!_activeModule.CanBePlaced())
            {
                return;
            }

            _activeModule.PlaceAtActiveArea();
            _activeModule = null;
        }

        #endregion
    }
}