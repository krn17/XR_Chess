using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintManager : MonoBehaviour
{
	public GameObject hintObject;
	
	private TextMeshProUGUI hintText;
	private Image hintImage;

	private void Start() {
		hintObject.SetActive(false);
		hintText = hintObject.GetComponentInChildren<TextMeshProUGUI>();
		hintImage = hintObject.GetComponentInChildren<Image>();
	}

	public void ShowHint(string text, Sprite icon) {
		Debug.Log(hintImage);
		hintObject.SetActive(true);
		hintText.text = text;
		hintImage.overrideSprite = icon;
	}

	public void HideHint() {
		hintObject.SetActive(false);
	}
}
