using Hackcreeper.LD54.Robot.Signals;
using Hackcreeper.LD54.Robot.Systems;
using UniDi;

namespace Hackcreeper.LD54.Robot.Di
{
    public class RobotInstaller : MonoInstaller<RobotInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<ModuleAttachedSignal>();

            Container.Bind<RobotLimit>().AsSingle();
        }
    }
}