using ContainerTree.Automation;
using Zenject;

namespace ContainerTree.Installers
{
    internal class ContainerTreeMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AutomationPlayer>().AsSingle();
        }
    }
}