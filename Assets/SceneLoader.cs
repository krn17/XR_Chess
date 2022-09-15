using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
	public Image loadingIndicatorImage;
	public GameObject loadingPanel;
	public GameObject[] disableOnLoading;

	public void LoadSceneAsync(string sceneName) {
		StartCoroutine(LoadAsynchronously(sceneName));
	}

	IEnumerator LoadAsynchronously(string sceneName) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

		loadingPanel.SetActive(true);
		for(int i=0; i<disableOnLoading.Length; i++) {
			disableOnLoading[i].SetActive(false);
		}
		
		while(!operation.isDone) {
			float progress = Mathf.Clamp01(operation.progress / .9f);

			loadingIndicatorImage.fillAmount = progress;
			yield return null;
		}
	}
}
