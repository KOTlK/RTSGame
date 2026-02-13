using System;
using UnityEngine.InputSystem;

public enum GameState {
	Initialization = 0,
	MainMenu       = 1,
	Gameplay       = 2,
	Pause          = 3,
}

public class GameStateSystem : GameSystem {
	private GameState _currentState;

	public GameStateSystem(Game game) : base(game, true) {
		_currentState = GameState.Initialization;
	}

	public override void OnEnable() {
		Events.SubGeneral<InitializationCompletedEvent>(OnInitializationOver);
		Events.SubGeneral<LevelLoadedEvent>(OnLevelLoad);
	}

    public override void OnDisable() {
		Events.UnsubGeneral<InitializationCompletedEvent>(OnInitializationOver);
		Events.UnsubGeneral<LevelLoadedEvent>(OnLevelLoad);
	}

    private void OnInitializationOver(InitializationCompletedEvent evnt) {
        var ls = Game.GetSystem<LevelSystem>();
        ls.LoadLevel(Config.StartLevel);
    }

    private void ChangeState(GameState newState) {
    	var evnt = new GameStateChangedEvent();

    	evnt.OldState = _currentState;
    	evnt.NewState = newState;

    	_currentState = newState;

    	Events.RaisePrivate("game_state", evnt);
    }

    private void OnLevelLoad(LevelLoadedEvent evnt) {
    	// @Cleanup: Move to input system;
    	InputSystem.actions.FindActionMap("Player").Enable();
    	ChangeState(GameState.Gameplay);
		Game.EnableSystem<CameraSystem>();
    }
}