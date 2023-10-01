using Hackcreeper.LD54.Robot.Components;
using UnityEngine;

namespace Hackcreeper.LD54.Robot.Signals
{
    public class ModuleAttachedSignal
    {
        public readonly RobotModule Module;
        public readonly Vector3Int Coordinates;

        public ModuleAttachedSignal(RobotModule module, Vector3Int coordinates)
        {
            Module = module;
            Coordinates = coordinates;
        }
    }
}