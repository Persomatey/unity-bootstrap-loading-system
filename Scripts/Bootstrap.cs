using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
	private bool overrideMenuFlow = false;
	private const string OverrideIntendedMenuFlowKey = "OverrideIntendedMenuFlowKey";
	private const string OverrideScenePathKey = "OverrideScenePath";

	private void Start()
	{
		Debug.Log("Bootstrap"); 

		Debug.Log(
			"Starting game\n" +
			$"identifier: {Application.identifier}\n" +
			$"platform: {Application.platform.ToString()}"
			);

		#if UNITY_EDITOR
			overrideMenuFlow = UnityEditor.EditorPrefs.GetBool(OverrideIntendedMenuFlowKey, false);
		#endif

		if (overrideMenuFlow)
		{
			Debug.Log("Overriding intended menu flow");

			#if UNITY_EDITOR
				string overrideScenePath = UnityEditor.EditorPrefs.GetString(OverrideScenePathKey, "");
			#endif
		}
		else
		{
  			Debug.Log("Proceeding with intended menu flow");
		}

		StartCoroutine(WaitUntilSingletonsHaveLoaded());
	}

	private IEnumerator WaitUntilSingletonsHaveLoaded()
	{
		yield return new WaitForEndOfFrame(); 

		//while (!SaveSystem.Instance.initialized)
		//{
		//	yield return new WaitForEndOfFrame();
		//}
    }
}