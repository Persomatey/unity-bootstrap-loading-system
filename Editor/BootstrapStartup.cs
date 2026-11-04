using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class BootstrapStartup
{
	private const string BootstrapSceneKey = "BootstrapScenePath";

	static BootstrapStartup()
	{
		EditorApplication.delayCall += ApplyBootstrapScene;
	}

	private static void ApplyBootstrapScene()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode)
		{
			return;
		}

		string scenePath = EditorPrefs.GetString(BootstrapSceneKey, "");

		if (string.IsNullOrEmpty(scenePath))
		{
			return;
		}

		SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

		if (scene == null)
		{
			Debug.LogWarning("Saved Bootstrap scene not found: " + scenePath);
			return;
		}

		EditorSceneManager.playModeStartScene = scene;
	}
}
