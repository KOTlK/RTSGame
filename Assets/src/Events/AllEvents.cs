using UnityEngine;

public struct InitializationCompletedEvent {
}

public struct LevelLoadBeginEvent {
}

public struct LevelLoadedEvent {
	public LevelDescription Description;
}

public struct GameStateChangedEvent {
	public GameState OldState;
	public GameState NewState;
}

public struct CameraMovedEvent {
	public Vector3 Delta;
}

public struct CameraRotatedEvent {
	public Vector2 Delta;
}