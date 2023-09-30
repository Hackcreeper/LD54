using Hackcreeper.LD54.Robot.Enums;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Data
{
    [CreateAssetMenu(fileName = "Module", menuName = "Custom/Module", order = 1)]
    public class ModuleSo : ScriptableObject
    {
        public ModuleType type;
        public GameObject prefab;
    }
}