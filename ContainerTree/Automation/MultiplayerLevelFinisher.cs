using SiraUtil.Zenject;
using System.Threading;
using System.Threading.Tasks;

namespace ContainerTree.Automation
{
    internal class MultiplayerLevelFinisher : IAsyncInitializable
    {
        private readonly IAudioTimeSource _audioTimeSource;
        private readonly MultiplayerLocalActivePlayerInGameMenuViewController _multiplayerLocalActivePlayerInGameMenuViewController;

        public MultiplayerLevelFinisher(IAudioTimeSource audioTimeSource, MultiplayerLocalActivePlayerInGameMenuViewController multiplayerLocalActivePlayerInGameMenuViewController)
        {
            _audioTimeSource = audioTimeSource;
            _multiplayerLocalActivePlayerInGameMenuViewController = multiplayerLocalActivePlayerInGameMenuViewController;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            while (_audioTimeSource.songTime <= 1f)
                await Task.Delay(10);
            _multiplayerLocalActivePlayerInGameMenuViewController.GiveUpButtonPressed();
        }
    }
}