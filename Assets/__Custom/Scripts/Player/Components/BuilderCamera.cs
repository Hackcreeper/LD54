using UnityEngine;

namespace Hackcreeper.LD54.Player.Components
{
    public class BuilderCamera : MonoBehaviour
    {
        #region EXPOSED FIELDS

        [SerializeField] private float rotationSpeed = 40f;
        [SerializeField] private float distance = 10f;
        [SerializeField] private float scrollSpeed = 2.5f;

        #endregion

        #region VARIABLES

        private Vector3 _lastMousePosition;
        private Vector2 _rotationAngle = new(0, 10.75f);
        private bool _firstFrame = true;

        #endregion 

        #region UPDATE

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                if (!_firstFrame)
                {
                    var deltaX = (_lastMousePosition.x - Input.mousePosition.x) * Time.deltaTime;
                    var deltaY = (Input.mousePosition.y - _lastMousePosition.y) * Time.deltaTime;
                    
                    _rotationAngle.x += deltaX;
                    _rotationAngle.y += deltaY;
                    
                    const float minPitch = 8f;
                    const float maxPitch = 13f;

                    _rotationAngle.y = Mathf.Clamp(_rotationAngle.y, minPitch, maxPitch);
                }

                _firstFrame = false;
                _lastMousePosition = Input.mousePosition;
            }
            else
            {
                _firstFrame = true;
            }

            distance += Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;
            distance = Mathf.Clamp(distance, 3f, 15f);

            var r = distance;
            var theta = _rotationAngle.y * Mathf.Deg2Rad * rotationSpeed;
            var phi = _rotationAngle.x * Mathf.Deg2Rad * rotationSpeed;
            
            transform.position = new Vector3(
                r * Mathf.Sin(theta) * Mathf.Cos(phi),
                r * Mathf.Cos(theta),
                r * Mathf.Sin(theta) * Mathf.Sin(phi)
            );
        }

        #endregion
    }
}