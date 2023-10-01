using Hackcreeper.LD54.Ui.Signals;
using UniDi;

namespace Hackcreeper.LD54.Ui.Di
{
    public class UiInstaller : MonoInstaller<UiInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<ModuleSelectedSignal>();
        }
    }
}