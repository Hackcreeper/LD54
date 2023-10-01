using System;
using Hackcreeper.LD54.Robot.Enums;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Data
{
    [Serializable]
    public struct SideVector3Struct
    {
        public AttachmentSide side;
        public Vector3 scale;
    }
}