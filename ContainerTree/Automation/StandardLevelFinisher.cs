using SiraUtil.Zenject;
using System.Threading.Tasks;

namespace ContainerTree.Automation
{
    internal class StandardLevelFinisher : IAsyncInitializable
    {
        private readonly IReturnToMenuController _returnToMenuController;

        public StandardLevelFinisher(IReturnToMenuController returnToMenuController)
        {
            _returnToMenuController = returnToMenuController;
        }

        public async Task InitializeAsync()
        {
            await Task.Delay(1000);
            _returnToMenuController.ReturnToMenu();
        }
    }
}