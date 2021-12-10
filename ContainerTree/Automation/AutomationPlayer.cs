using SiraUtil.Zenject;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace ContainerTree.Automation
{
    internal class AutomationPlayer : IAsyncInitializable
    {
        //private readonly SiraLog _siraLog;
        //private readonly PlayerDataModel _playerDataModel;
        //private readonly BeatmapLevelsModel _beatmapLevelModel;
        //private readonly MenuTransitionsHelper _menuTransitionsHelper;
        //private readonly ILobbyPlayersDataModel _lobbyPlayersDataModel;
        //private readonly MainMenuViewController _mainMenuViewController;
        //private readonly CreateServerViewController _createServerViewController;
        //private readonly ILobbyHostGameStateController _lobbyHostGameStateController;
        //private readonly MultiplayerModeSelectionViewController _multiplayerModeSelectionViewController;
        //
        //public AutomationPlayer(SiraLog siraLog, PlayerDataModel playerDataModel, BeatmapLevelsModel beatmapLevelModel, MenuTransitionsHelper menuTransitionsHelper, ILobbyPlayersDataModel lobbyPlayersDataModel, MainMenuViewController mainMenuViewController, CreateServerViewController createServerViewController, ILobbyHostGameStateController lobbyHostGameStateController, MultiplayerModeSelectionViewController multiplayerModeSelectionViewController)
        //{
        //    _siraLog = siraLog;
        //    _playerDataModel = playerDataModel;
        //    _beatmapLevelModel = beatmapLevelModel;
        //    _menuTransitionsHelper = menuTransitionsHelper;
        //    _lobbyPlayersDataModel = lobbyPlayersDataModel;
        //    _mainMenuViewController = mainMenuViewController;
        //    _createServerViewController = createServerViewController;
        //    _lobbyHostGameStateController = lobbyHostGameStateController;
        //    _multiplayerModeSelectionViewController = multiplayerModeSelectionViewController;
        //}

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            //_siraLog.Debug("Initializing...");
            //bool menuLoaded = false;
            //_mainMenuViewController.didActivateEvent += delegate (bool _, bool __, bool ___)
            //{
            //    menuLoaded = true;
            //};
            //await WaitUntil(() => menuLoaded);
            //_siraLog.Debug("Menu UI Loaded");
            //await Task.Delay(500);
            //
            //_siraLog.Debug("Looking for test level...");
            //IPreviewBeatmapLevel previewBeatmapLevel = _beatmapLevelModel.allLoadedBeatmapLevelPackCollection.beatmapLevelPacks[0].beatmapLevelCollection.beatmapLevels[0];
            //_siraLog.Debug($"Level: {previewBeatmapLevel.levelID}");
            //
            //BeatmapLevelSO levelSO = (previewBeatmapLevel as BeatmapLevelSO)!;
            //IDifficultyBeatmap difficultyBeatmap = levelSO.beatmapLevelData.difficultyBeatmapSets[0].difficultyBeatmaps[0];
            //
            //_siraLog.Debug("Starting standard level...");
            //_menuTransitionsHelper.StartStandardLevel("Solo", difficultyBeatmap, previewBeatmapLevel,
            //    _playerDataModel.playerData.overrideEnvironmentSettings,
            //    _playerDataModel.playerData.colorSchemesSettings.GetColorSchemeForId(_playerDataModel.playerData.colorSchemesSettings.selectedColorSchemeId),
            //    _playerDataModel.playerData.gameplayModifiers,
            //    _playerDataModel.playerData.playerSpecificSettings,
            //    _playerDataModel.playerData.practiceSettings,
            //    "Menu", false, null, null);
            //
            //await Task.Delay(1000);
            //await WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenu");
            //_siraLog.Debug("Returned back to menu.");
            //await Task.Delay(500);
            //_siraLog.Debug("Starting campaign level...");
            //
            //_menuTransitionsHelper.StartMissionLevel("Test", difficultyBeatmap, previewBeatmapLevel,
            //    _playerDataModel.playerData.colorSchemesSettings.GetColorSchemeForId(_playerDataModel.playerData.colorSchemesSettings.selectedColorSchemeId),
            //    _playerDataModel.playerData.gameplayModifiers,
            //    Array.Empty<MissionObjective>(),
            //    _playerDataModel.playerData.playerSpecificSettings,
            //    null, null);
            //
            //await Task.Delay(1000);
            //await WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenu");
            //_siraLog.Debug("Returned back to menu.");
            //
            //await Task.Delay(500);
            //_siraLog.Debug("Starting tutorial...");
            //_menuTransitionsHelper.StartTutorial();
            //await Task.Delay(1000);
            //await WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenu");
            //_siraLog.Debug("Returned back to menu.");
            //await Task.Delay(500);
            //
            //_siraLog.Debug("Navigation to multiplayer...");
            //_mainMenuViewController.HandleMenuButton(MainMenuViewController.MenuButton.Multiplayer);
            //await Task.Delay(1250);
            //_multiplayerModeSelectionViewController.HandleMenuButton(MultiplayerModeSelectionViewController.MenuButton.CreateServer);
            //await Task.Delay(750);
            //FieldAccessor<CreateServerViewController, Action<bool, UnifiedNetworkPlayerModel.CreatePartyConfig>>.Get(_createServerViewController, "didFinishEvent").Invoke(true, _createServerViewController.CreatePartyConfig());
            //await Task.Delay(750);
            //_lobbyPlayersDataModel.SetLocalPlayerBeatmapLevel(previewBeatmapLevel.levelID, difficultyBeatmap.difficulty, difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
            //await Task.Delay(750);
            //_siraLog.Debug("Starting multiplayer level...");
            //_lobbyHostGameStateController.StartGame();
            //await Task.Delay(1000);
            //await WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenu");
            await Task.Delay(1000);
            Application.Quit();
        }

        private async Task WaitUntil(Func<bool> until)
        {
            while (!until.Invoke())
                await Task.Delay(10);
        }
    }
}