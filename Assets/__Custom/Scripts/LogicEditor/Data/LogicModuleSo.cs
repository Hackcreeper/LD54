using UnityEngine;

namespace Hackcreeper.LD54.LogicEditor.Data
{
    [CreateAssetMenu(fileName = "Logic Module", menuName = "Custom/Logic Module", order = 2)]
    public class LogicModuleSo : ScriptableObject
    {
        public string label;
        public GameObject uiPrefab;
    }
}