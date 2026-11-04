using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BootstrapWindow : EditorWindow
{
	private const string BootstrapSceneKey = "BootstrapScenePath";
	private const string DefaultBootstrapScene = "Assets/Scenes/Bootstrap.unity";

	private const string OverrideIntendedMenuFlowKey = "OverrideIntendedMenuFlowKey";
	private bool OverrideMenuFlow;

	private void OnEnable()
	{
		OverrideMenuFlow = EditorPrefs.GetBool(OverrideIntendedMenuFlowKey, false);
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("Bootstrap Settings", EditorStyles.boldLabel);

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

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Scene Flow Settings", EditorStyles.boldLabel);
		EditorGUI.BeginChangeCheck();
		OverrideMenuFlow = EditorGUILayout.Toggle("Override Followup Scene", OverrideMenuFlow);
		if (EditorGUI.EndChangeCheck())
		{
			EditorPrefs.SetBool(OverrideIntendedMenuFlowKey, OverrideMenuFlow);
		}

		if (OverrideMenuFlow)
		{
			SceneAsset overrideScene = (SceneAsset)EditorGUILayout.ObjectField(
				new GUIContent("Override Scene"),
				AssetDatabase.LoadAssetAtPath<SceneAsset>(
					EditorPrefs.GetString("OverrideScenePath", "")
				),
				typeof(SceneAsset),
				false
			);

			if (overrideScene != null)
			{
				string path = AssetDatabase.GetAssetPath(overrideScene);
				EditorPrefs.SetString("OverrideScenePath", path);
			}

			if (GUILayout.Button("Use Current Open Scene"))
			{
				string currentScenePath = EditorSceneManager.GetActiveScene().path;

				SceneAsset currentScene =
					AssetDatabase.LoadAssetAtPath<SceneAsset>(currentScenePath);

				if (currentScene != null)
				{
					EditorPrefs.SetString("OverrideScenePath", currentScenePath);
				}
				else
				{
					Debug.LogError("Current scene is not a saved asset.");
				}
			}
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
		EditorPrefs.SetString(BootstrapSceneKey, scenePath);
	}

	[MenuItem("Bootstrap/Bootstrap Menu")]
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