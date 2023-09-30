using UnityEngine;

namespace Hackcreeper.LD54.Helper
{
    public class CustomCameraHelper
    {
        public static Transform? RayFromMouseToTarget(Camera camera, LayerMask layerMask)
        {
#if THEREALIRONDUCK_NEW_INPUT
            var ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
#else
            var ray = camera.ScreenPointToRay(Input.mousePosition);
#endif
            if (!Physics.Raycast(ray, out var hit, 1000f, layerMask))
            {
                return null;
            }

            return hit.transform;
        }
    }
}