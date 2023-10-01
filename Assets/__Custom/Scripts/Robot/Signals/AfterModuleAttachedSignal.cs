using Hackcreeper.LD54.Robot.Components;

namespace Hackcreeper.LD54.Robot.Signals
{
    public class AfterModuleAttachedSignal
    {
        public readonly RobotModule Module;

        public AfterModuleAttachedSignal(RobotModule module)
        {
            Module = module;
        }
    }
}