using UnityEngine;

namespace Hackcreeper.LD54.Robot.Data
{
    [CreateAssetMenu(fileName = "Module", menuName = "Custom/Module", order = 1)]
    public class ModuleSo : ScriptableObject
    {
        public string label;
        public int costs;
        public GameObject prefab;
    }
}