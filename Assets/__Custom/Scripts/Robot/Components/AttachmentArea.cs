using Hackcreeper.LD54.Robot.Enums;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Components
{
    public class AttachmentArea : MonoBehaviour
    {
        #region VARIABLES

        private RobotModule _parentModule;
        private AttachmentSide _side;

        #endregion
        
        #region PUBLIC METHODS
        
        public void Initialize(RobotModule parentModule, AttachmentSide side)
        {
            _parentModule = parentModule;
            _side = side;
        }

        public RobotModule GetParentModule() => _parentModule;

        public AttachmentSide GetSide() => _side;
        
        #endregion
    }
}