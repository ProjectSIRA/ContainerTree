using ContainerTree.Installers;
using IPA;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace ContainerTree
{
    [Slog, Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPALogger logger, Zenjector zenjector)
        {
            zenjector.UseLogger(logger);
            zenjector.Install<ContainerTreeCoreInstaller>(Location.App);
            zenjector.Install<ContainerTreeMenuInstaller>(Location.Menu);
            zenjector.Install<ContainerTreeGameInstaller>(Location.Menu);
        }

        [OnEnable]
        public void OnEnable()
        {

        }

        [OnDisable]
        public void OnDisable()
        {

        }
    }
}