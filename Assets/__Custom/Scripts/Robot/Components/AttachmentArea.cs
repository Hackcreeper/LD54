using Hackcreeper.LD54.Robot.Enums;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class AttachmentArea : MonoBehaviour
    {
        #region VARIABLES

        private RobotModule _parentModule;
        private AttachmentSide _side;
        private Vector3Int _coords;

        #endregion
        
        #region PUBLIC METHODS
        
        public void Initialize(RobotModule parentModule, AttachmentSide side, Vector3Int coords)
        {
            _parentModule = parentModule;
            _side = side;
            _coords = coords;
        }

        public RobotModule GetParentModule() => _parentModule;

        public AttachmentSide GetSide() => _side;

        public Vector3Int GetCoords() => _coords;


        #endregion
    }
}