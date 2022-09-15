using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour
{
	public float splashTime = 10f;
	public string sceneToLoad = "MenuScreen";

	private void Start() {
		Invoke("LoadMenuScreen", splashTime);
	}

	private void LoadMenuScreen() {
		SceneManager.LoadScene(sceneToLoad);
	} 
}
