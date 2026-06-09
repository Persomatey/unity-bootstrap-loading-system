using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HunterGoodin.Bootstrap
{
	public class BootstrapWindow : EditorWindow
	{
		private const string BootstrapSceneKey = "BootstrapScenePath";
		private const string DefaultBootstrapScene = "Assets/Scenes/Bootstrap.unity";

		void OnGUI()
		{
			EditorSceneManager.playModeStartScene =
				(SceneAsset)EditorGUILayout.ObjectField(
					new GUIContent("Start Scene"),
					EditorSceneManager.playModeStartScene,
					typeof(SceneAsset),
					false
				);

			if (GUILayout.Button("Set Bootstrap Start Scene"))
			{
				SetPlayModeStartScene(DefaultBootstrapScene);
			}
		}

		void SetPlayModeStartScene(string scenePath)
		{
			SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
			if (scene == null)
			{
				Debug.LogError("Could not find Scene " + scenePath);
				return;
			}

			EditorSceneManager.playModeStartScene = scene;

			// 🔒 Persist it
			EditorPrefs.SetString(BootstrapSceneKey, scenePath);
		}

		[MenuItem("Bootstrap/SetStartScene")]
		static void Open()
		{
			GetWindow<BootstrapWindow>("Bootstrap");
		}
	}

	[InitializeOnLoad]
	public static class PlayModeBootstrapSetup
	{
		static PlayModeBootstrapSetup()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		static void OnPlayModeChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingEditMode)
			{
				// Save the current open scene path before entering Play Mode
				var activeScene = EditorSceneManager.GetActiveScene();
				EditorPrefs.SetString("PreviousScenePath", activeScene.path);
			}
		}
	}
}