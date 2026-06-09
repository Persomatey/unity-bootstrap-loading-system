# Unity Bootstrap Loading System 
Lightweight and package-friendly Bootstrap scene architecture with minimal setup required that works cleanly inside Unity packages and can be integrated into existing projects with custom Bootstrap scenes. Reduces scene setup friction during development. No need to manually open persistent/system scenes additively. No need for duplicate manager prefabs in every scene. Keeps project-wide dependencies centralized and consistent

Check out [Releases](https://github.com/Persomatey/unity-bootstrap-loading-system/releases) tab to see a history of all versions of this package. 

## Installation 
### Install via Package Manager

<details>
<summary>Select `Install package from git URL...` in the Package Manager</summary>
	<img src="https://raw.githubusercontent.com/Persomatey/unity-package-ci-cd-system-template/refs/heads/main/images/git-url-installation-example.png">
</details>

For latest release:</br>
`https://github.com/Persomatey/unity-package-ci-cd-system-template.git#upm`</br>
Or install specific releases:</br>
`https://github.com/Persomatey/unity-package-ci-cd-system-template.git#v#.#.#`

### Download the tarball directly from release
<details>
<summary>Go to Release and download directly from Assets</summary>
	  <img src="https://raw.githubusercontent.com/Persomatey/unity-package-ci-cd-system-template/refs/heads/main/images/release-tab-tarball-circled.png">
</details>

`com.huntergoodin.bootstrap-v#.#.#.tgz`

`com.huntergoodin.bootstrap-v#.#.#.zip`

<details>
<summary>Select `Install package from tarball...` in the Package Manager</summary>
	  <img src="https://raw.githubusercontent.com/Persomatey/unity-package-ci-cd-system-template/refs/heads/main/images/tarball-installation-example.png">
</details>

## Set up 
1. If you're not going to use the provided `Bootstrap.unity` scene, make sure you drag the `Bootstrap.cs` script onto one of the GameObjects in your bootstrap scene 
2. In `Packages/com.huntergoodin.bootstrap/Editor/EditorScripts/StartSceneWindow.cs`, ensure you change the `DefaultBootstrapScene` (Line 8) to the path of your chosen Bootstrap scene 
3. The wait conditions in `Bootstrap.WaitUntilSingletonsHaveLoaded()` are optional, pick which one you want to use 
		- The package is immutable by default but you can always just Right Click > Show in Explorer/Finder/etc then edit it directly in a text editor 

## Features 
- Automatically forces a designated Bootstrap scene to load first in Play Mode	
	- Ensures required persistent systems and dependencies initialize before gameplay scenes
	- Prevents missing singleton/reference issues when testing scenes directly in-editor
- Remembers and restores the Play Mode Start Scene across Unity editor restarts 
	-  Toggleable in `Bootstrap.overloadToPreviousScenePath` 
		- Useful for release testing or menu-flow testing
 	- Eliminates the need to manually reassign the Bootstrap scene every session
	- Uses EditorPrefs + InitializeOnLoad for persistent editor setup
- Automatically returns to the previously opened scene after Bootstrap initialization
	- Lets you press Play from any scene while still initializing your core systems first
	- Seamlessly redirects back into your test scene once bootstrapping is complete
- Supports asynchronous singleton/dependency initialization workflows
	- Includes optional wait conditions before proceeding to the target scene
	- Drag-and-drop MonoBehaviours that must exist before scene transition
	- Easy to extend without modifying core bootstrap logic
	- Useful for:
		- Analytics systems
		- Save systems
		- Audio managers
		- Multiplayer/network initialization
		- Addressables/loading systems
- Build-compatible scene flow
	- Automatically loads MainMenu in standalone/player builds
	- Editor-only testing logic is wrapped in #if UNITY_EDITOR
- Includes editor tooling for Play Mode Start Scene management
	- Simple utility window for assigning the Bootstrap scene
	- Designed to streamline scene testing workflows during development

## Future Plans 
<i>No plans on when I'd release these features, would likely depend on my needs for a specific project/boredom/random interest in moving this project along.</i>
- Not sure... tbh, this is a pretty straightforward system. 
