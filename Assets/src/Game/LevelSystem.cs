using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using static Assertions;

public class LevelSystem : GameSystem {
	private LevelDatabase    					_levelDatabase;
	private LevelDescription 					_loadingLevel;
	private AsyncOperationHandle<SceneInstance> _loadHandle;

	private const string LevelsAsset = "Levels";

	public LevelSystem(Game game) : base(game, false) {
		_levelDatabase = AssetManager.Load<LevelDatabase>(LevelsAsset);
	}

	public void LoadLevel(string name) {
		foreach(var level in _levelDatabase.Levels) {
			if (level.Name == name) {
				_loadingLevel = level;
				_loadHandle = Addressables.LoadSceneAsync(level.Scene, 
					 									  LoadSceneMode.Single);

				_loadHandle.Completed += OnSceneLoad;
				Events.RaiseGeneral(new LevelLoadBeginEvent());
				return;
			}
		}

		Assert(false, "Cannot load level by name (%). Level does not exist.", name);
	}

	public float GetLevelLoadProgress() {
		return _loadHandle.PercentComplete;
	}

	public void GetAllAvailableLevels(List<LevelDescription> buffer) {
		foreach(var level in _levelDatabase.Levels) {
			buffer.Add(level);
		}
	}

    private void OnSceneLoad(AsyncOperationHandle<SceneInstance> handle) {
        var evnt  = new LevelLoadedEvent();
        var descr = _loadingLevel;

        descr.Terrain    = Object.FindAnyObjectByType<Terrain>();
        descr.Camera     = Object.FindAnyObjectByType<Camera>();
        evnt.Description = descr;

        Assert(descr.Terrain, "Cannot find terrain on the level.");
        Assert(descr.Camera, "Cannot find camera on the level.");

        Events.RaiseGeneral(evnt);
    }
}