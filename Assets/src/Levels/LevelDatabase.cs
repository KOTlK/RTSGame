using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public struct LevelDescription {
			   public string 		 Name;
			   public AssetReference Scene;
	[ReadOnly] public Terrain        Terrain;
	[ReadOnly] public Camera 		 Camera;
}

[CreateAssetMenu(menuName = "New/LevelDatabase")]
public class LevelDatabase : ScriptableObject {
	public LevelDescription[] Levels;
}