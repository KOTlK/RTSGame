using UnityEngine;

public class TerrainSystem : GameSystem {
	private Terrain _terrain;

	public TerrainSystem(Game game) : base(game, true) {
	}

    public override void OnEnable() {
    	Events.SubGeneral<LevelLoadedEvent>(OnLevelLoad);
    }

    public override void OnDisable() {
		Events.UnsubGeneral<LevelLoadedEvent>(OnLevelLoad);
    }

    public float SampleHeight(Vector3 position) {
    	return _terrain.SampleHeight(position);
    }

    private void OnLevelLoad(LevelLoadedEvent evnt) {
    	_terrain = evnt.Description.Terrain;
    }
}