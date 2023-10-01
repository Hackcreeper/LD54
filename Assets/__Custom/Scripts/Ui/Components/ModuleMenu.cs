using Hackcreeper.LD54.Robot.Components;
using UnityEngine;

namespace Hackcreeper.LD54.Ui.Components
{
    public class ModuleMenu : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private RobotBuilder robotBuilder;
        [SerializeField] private GameObject moduleButtonPrefab;

        #endregion

        #region LIFECYCLE METHODS

        private void Start()
        {
            foreach (var module in robotBuilder.GetAvailableModules())
            {
                var button = Instantiate(moduleButtonPrefab, transform);
                button.GetComponent<ModuleButton>().Initialize(module);
            }
        }

        #endregion
    }
}