using Hackcreeper.LD54.Helper;
using Hackcreeper.LD54.Robot.Data;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class RobotBuilder : MonoBehaviour
    {
        #region CONSTS

        public static RobotBuilder Instance { get; private set; }

        #endregion

        #region EXPOSED FIELDS

        [SerializeField] private ModuleSo coreBlock;
        [SerializeField] private ModuleSo[] availableModules;
        [SerializeField] private LayerMask attachmentAreaLayerMask;

        #endregion
        
        #region VARIABLES

        private RobotModule _activeModule;

        #endregion
        
        #region LIFECYCLE METHODS
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            
            Instance = this;
        }

        private void Start()
        {
            var module = Instantiate(coreBlock.prefab);
            module.GetComponent<RobotModule>().Place(Vector3.zero);
        }

        private void Update()
        {
            if (_activeModule)
            {
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
                _activeModule.LockInPlace(target.position);
                return;
            }
            
            _activeModule.EnablePlaceholderMode();
        }

        #endregion
    }
}