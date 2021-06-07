using Zenject;

namespace ContainerTree.Installers
{
    internal class ContainerTreeCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TreeBuilder>().AsSingle();
            Container.BindInterfacesTo<TreeAgent>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}