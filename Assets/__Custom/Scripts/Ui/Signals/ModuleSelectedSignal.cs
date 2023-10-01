using Hackcreeper.LD54.Robot.Data;

namespace Hackcreeper.LD54.Ui.Signals
{
    public class ModuleSelectedSignal
    {
        public readonly ModuleSo Module;

        public ModuleSelectedSignal(ModuleSo module)
        {
            Module = module;
        }
    }
}