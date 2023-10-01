using Hackcreeper.LD54.LogicEditor.Signals;
using UniDi;

namespace Hackcreeper.LD54.LogicEditor.Di
{
    public class LogicEditorInstaller : MonoInstaller<LogicEditorInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<LogicEditorToggledSignal>();
        }
    }
}