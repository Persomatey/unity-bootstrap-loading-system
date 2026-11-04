using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HunterGoodin.Bootstrap
{
	public class Bootstrap : MonoBehaviour
	{
		private bool overloadToPreviousScenePath = true;
		[SerializeField] private string nextSceneName = "MainMenu";
		[SerializeField] private MonoBehaviour[] singletonInitializers; // Assign any MonoBehaviours that initialize singletons in the inspector

		private void Start()
		{
			Debug.Log("Bootstrap");

			Debug.Log(
				"Starting game\n" +
				$"identifier: {Application.identifier}\n" +
				$"platform: {Application.platform.ToString()}"
				);

			StartCoroutine(WaitUntilSingletonsHaveLoaded());
		}

		private IEnumerator WaitUntilSingletonsHaveLoaded()
		{
			// Wait until all singleton initializers have completed their initialization (assuming they set themselves to null or some other state when done) 
			if (singletonInitializers.Length != 0)
			{
				for (int i = 0; i < singletonInitializers.Length; i++)
				{
					while (!singletonInitializers[i])
					{
						yield return new WaitForEndOfFrame();
					}
				}
			}

			// Or if there's some other custom initialization condition built in, you can do that too 
			//while (!AnalyticsSystem.Instance.initialized)
			//{
			//    yield return new WaitForEndOfFrame();
			//}

			#if UNITY_EDITOR
				string previousScenePath = UnityEditor.EditorPrefs.GetString("PreviousScenePath", ""); // Fallback if not found

				// Determine which scene to proceed to 
				if (overloadToPreviousScenePath && !string.IsNullOrEmpty(previousScenePath)) // If there was already a previous scene, load directly into that instead
				{
					Debug.Log($"Previous scene path found, proceeding to {previousScenePath}");
					string sceneName = System.IO.Path.GetFileNameWithoutExtension(previousScenePath);
					SceneManager.LoadScene(sceneName);
				}
				else // Otherwise, just go straight to the main menu 
				{
					Debug.LogError($"No previous scene path found, proceeding to {nextSceneName}");

					//HunterGoodin.SceneBridge.SceneBridgeLoader.LoadSceneAsynchronouslyWithLoadingScreenAndTransition(nextSceneName, 0, 0, 0, 0); // If you're using SceneBridge, you can do that too 
					SceneManager.LoadScene(nextSceneName); // Fallback if somehow missing
				}
			#endif
			#if !UNITY_EDITOR
				//HunterGoodin.SceneBridge.SceneBridgeLoader.LoadSceneAsynchronouslyWithLoadingScreenAndTransition(nextSceneName, 0, 0, 0, 0); // If you're using SceneBridge, you can do that too 
				SceneManager.LoadScene(nextSceneName); // Fallback if somehow missing
			#endif
		}
	}
}