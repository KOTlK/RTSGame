using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class LevelTools : EditorWindow {
    [SerializeField] public Object TargetScene;
    [MenuItem("Tools/Level Tools", false, 10)]
    public static void ShowWindow() {
        GetWindow<LevelTools>("Level Tools", true);
    }

    private void OnGUI() {
        TargetScene = (SceneAsset)EditorGUILayout.ObjectField("Initialization scene asset",
                                                                TargetScene,
                                                                typeof(SceneAsset),
                                                                false
        );
        if (GUILayout.Button("Open Initialization Scene", GUILayout.Height(40))) {
            OpenInitializationScene();
        }
    }

    private void OpenInitializationScene() {
        if (TargetScene == null) {
            EditorUtility.DisplayDialog(
                "No scene selected",
                "Please drag a scene into the field above.",
                "OK"
            );
            return;
        }

        string scenePath = AssetDatabase.GetAssetPath(TargetScene);

        if (string.IsNullOrEmpty(scenePath)) {
            Debug.LogWarning("Cannot get path from selected SceneAsset.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
            return;
        }

        EditorSceneManager.OpenScene(scenePath);
    }
}