using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JMRSDK.Toolkit;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using ChessAR;
using JMRSDK.InputModule;
using JMRSDK.Toolkit.UI;

public class MenuManager : MonoBehaviour, IBackHandler
{
	enum MenuFocus {
		Home,
		Tutorial,
		Settings,
		Profile,
		Leaderboard,
		Difficulty
	}
	public string gameScene = "Sandbox";
	public AudioClip menuMusic;
	[Header("Menu Home")]
	public RectTransform homeMenuPanel;
	public JMRUIPrimaryButton playButton;
	public JMRUIPrimaryButton tutorialBtn;
	public JMRUIPrimaryButton leaderboardBtn;
	public JMRUIPrimaryButton settingsBtn;
	public JMRUIPrimaryButton profileBtn;
	
	[Header("Welcome Panel")]
	public RectTransform welcomePanel;
	public JMRUIPrimaryInputField welcomeNameInput;
	public JMRUIPrimaryButton welcomeSubmitBtn;
	public TextMeshProUGUI profileBtnNameText;
	
	[Header("Profile Panel")]
	public RectTransform profilePanel;
	public JMRUIPrimaryButton profileBackBtn;
	public TextMeshProUGUI profileNameText;
	public TextMeshProUGUI profileHighscoreText;

	[Header("Settings Panel")]
	public RectTransform settingsPanel;
	public JMRUIPrimaryButton settingsBackBtn;
	public TMP_Dropdown difficultyDropdown;
	public Slider musicSlider;
	public Slider sfxSlider;

	[Header("Leaderboard Panel")]
	public RectTransform leaderboardPanel;
	public JMRUIPrimaryButton leaderboardBackBtn;

	[Header("Difficulty Panel")]
	public RectTransform difficultyPanel;

	[Header("Tutorial Panel")]
	public RectTransform tutorialPanel;
	public JMRUIPrimaryButton tutorialBackBtn;
	public JMRUIPrimaryButton previousHelpBtn;
	public JMRUIPrimaryButton nextHelpBtn;
	public RectTransform[] tutorialHelpPanels;
	private int _activeHelpIndex = 0;


	private MenuFocus focusedMenu = MenuFocus.Home;

	private void Start() {
		JMRInputManager.Instance.AddGlobalListener(gameObject);
		InitializeSaveData();

		playButton.OnClick.AddListener(() => {
			HideHomePanel();
			ShowDifficultyPanel();
		});
		// profile panel
		profileBtn.OnClick.AddListener(() => {
			HideHomePanel();
			ShowProfilePanel();
		});
		profileBackBtn.OnClick.AddListener(() => {
			ShowHomePanel();
			HideProfilePanel();
		});
		// settings panel
		settingsBtn.OnClick.AddListener(() => {
			HideHomePanel();
			ShowSettingsPanel();
		});
		settingsBackBtn.OnClick.AddListener(() => {
			ShowHomePanel();
			HideSettingsPanel();
		});
		// COnfigure leaderboardBtn panel buttons
		leaderboardBtn.OnClick.AddListener(() => {
			HideHomePanel();
			ShowLeaderboardPanel();
		});
		leaderboardBackBtn.OnClick.AddListener(() => {
			ShowHomePanel();
			HideLeaderboardPanel();
		});

		int difficulty = PlayerPrefs.GetInt("game_difficulty", 3);
		Debug.Log("Difficulty: " + difficulty);
		difficultyDropdown.value = difficulty-1;
		difficultyDropdown.onValueChanged.AddListener(OnDifficultyChange);

		// configure music volume slider
		musicSlider.value = AudioManager.instance.musicVolumePercent;
		musicSlider.onValueChanged.AddListener((float value) => {
			AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
		});
		// configure sfx volume slider
		sfxSlider.value = AudioManager.instance.sfxVolumePercent;
		sfxSlider.onValueChanged.AddListener((float value) => {
			AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
		});

		// COnfigure tutorial panel buttons
		tutorialBtn.OnClick.AddListener(() => {
			HideHomePanel();
			ShowTutorialPanel();
		});
		tutorialBackBtn.OnClick.AddListener(() => {
			ShowHomePanel();
			HideTutorialPanel();
		});
		previousHelpBtn.OnClick.AddListener(ShowPreviousHelp);
		nextHelpBtn.OnClick.AddListener(ShowNextHelp);

		// Play Music
		AudioManager.instance.PlayMusic(menuMusic);
	}

	private void InitializeSaveData() {
		SaveData data = SaveManager.Load();
		// if we dont have a player name yet, show the welcome screen
		if(data.saveObject.playerName == "") {
			ShowWelcomePanel();
			HideHomePanel();
			welcomeSubmitBtn.OnClick.AddListener(SubmitPlayerName);
		}
		else {
			profileBtnNameText.text = data.saveObject.playerName;
		}
	}

	private void SubmitPlayerName() {
		string name = welcomeNameInput.Text.Trim();
		if(name == "") return;
		// save player name
		SaveManager.SaveName(name);
		profileBtnNameText.text = name;
		HideWelcomePanel();
		ShowHomePanel();
	}

	private void ShowWelcomePanel() {
		//focusedMenu = MenuFocus.Welcome;
		welcomePanel.gameObject.SetActive(true);
		welcomePanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
	}
	private void HideWelcomePanel() {
		welcomePanel.GetComponent<CanvasGroup>().alpha = 0f;
		welcomePanel.gameObject.SetActive(false);
	}

	public void LoadGameScene(int difficulty) {
		PlayerPrefs.SetInt("game_difficulty", difficulty);
		Debug.Log("New difficulty: " + difficulty);

		GetComponent<SceneLoader>().LoadSceneAsync(gameScene);
	}

	private void ShowHomePanel() {
		focusedMenu = MenuFocus.Home;
		homeMenuPanel.gameObject.SetActive(true);
		homeMenuPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
	}
	private void HideHomePanel() {
		homeMenuPanel.GetComponent<CanvasGroup>().alpha = 0f;
		homeMenuPanel.gameObject.SetActive(false);
	}
	private void ShowProfilePanel() {
		focusedMenu = MenuFocus.Profile;
		SaveData saveData = SaveManager.Load();
		profileNameText.text = saveData.saveObject.playerName;
		profileHighscoreText.text = saveData.saveObject.highscore + "";
		profilePanel.gameObject.SetActive(true);
		profilePanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		profilePanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}
	private void HideProfilePanel() {
		profilePanel.GetComponent<CanvasGroup>().alpha = 0f;
		profilePanel.gameObject.SetActive(false);
	}
	private void ShowDifficultyPanel() {
		focusedMenu = MenuFocus.Difficulty;

		difficultyPanel.gameObject.SetActive(true);
		difficultyPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		difficultyPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}
	private void HideDifficultyPanel() {
		difficultyPanel.GetComponent<CanvasGroup>().alpha = 0f;
		difficultyPanel.gameObject.SetActive(false);
	}

	private void ShowSettingsPanel() {
		focusedMenu = MenuFocus.Settings;
		// settingsPanel.DOScale(Vector3.one, 03f);
		settingsPanel.gameObject.SetActive(true);
		settingsPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		settingsPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}
	private void HideSettingsPanel() {
		// settingsPanel.DOScale(Vector3.one, 03f);
		settingsPanel.GetComponent<CanvasGroup>().alpha = 0f;
		settingsPanel.gameObject.SetActive(false);
		// settingsPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		// settingsPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}
	private void ShowLeaderboardPanel() {
		focusedMenu = MenuFocus.Leaderboard;

		leaderboardPanel.gameObject.SetActive(true);
		leaderboardPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		leaderboardPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}
	private void HideLeaderboardPanel() {
		leaderboardPanel.GetComponent<CanvasGroup>().alpha = 0f;
		leaderboardPanel.gameObject.SetActive(false);
	}

	private void ShowTutorialPanel() {
		// make sure first help is active
		foreach(RectTransform helpPanel in tutorialHelpPanels) {
			helpPanel.gameObject.SetActive(false);
		}
		tutorialHelpPanels[0].gameObject.SetActive(true);
		// if(_activeHelpIndex <= 0)
		// 	previousHelpBtn.interactable = false;
		// if(_activeHelpIndex < tutorialHelpPanels.Length - 1)
		// 	nextHelpBtn.interactable = true;

		focusedMenu = MenuFocus.Tutorial;
		// settingsPanel.DOScale(Vector3.one, 03f);
		tutorialPanel.gameObject.SetActive(true);
		tutorialPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		tutorialPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}

	private void ShowPreviousHelp() {
		// disable the currently active help panel
		tutorialHelpPanels[_activeHelpIndex].gameObject.SetActive(false);
		// set new help index
		_activeHelpIndex = _activeHelpIndex <= 0 ? 0 : _activeHelpIndex - 1;
		// enable the new help panel
		tutorialHelpPanels[_activeHelpIndex].gameObject.SetActive(true);

		// if(_activeHelpIndex <= 0)
		// 	previousHelpBtn.interactable = false;
		// if(_activeHelpIndex < tutorialHelpPanels.Length - 1)
		// 	nextHelpBtn.interactable = true;
		
	}
	private void ShowNextHelp() {
		// disable the currently active help panel
		tutorialHelpPanels[_activeHelpIndex].gameObject.SetActive(false);
		// set new help index
		_activeHelpIndex = _activeHelpIndex >= tutorialHelpPanels.Length - 1 ? tutorialHelpPanels.Length - 1 : _activeHelpIndex + 1;
		// enable the new help panel
		tutorialHelpPanels[_activeHelpIndex].gameObject.SetActive(true);

		// if(_activeHelpIndex >= tutorialHelpPanels.Length - 1)
		// 	nextHelpBtn.interactable = false;
		// if(_activeHelpIndex > 0) 
		// 	previousHelpBtn.interactable = true;
		
	}
	private void HideTutorialPanel() {

		// settingsPanel.DOScale(Vector3.one, 03f);
		tutorialPanel.GetComponent<CanvasGroup>().alpha = 0f;
		tutorialPanel.gameObject.SetActive(false);
		// settingsPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f);
		// settingsPanel.DOShakeScale(0.3f,0.2f, 20, 90f);
	}

	void OnDifficultyChange(int value) {
		int newDifficulty = value + 1;
		PlayerPrefs.SetInt("game_difficulty", newDifficulty);
		Debug.Log("New difficulty: " + newDifficulty);
	}

	public void OnBackAction()
	{
		switch(focusedMenu) {
			case MenuFocus.Settings:
				HideSettingsPanel();
				ShowHomePanel();
				break;
			case MenuFocus.Leaderboard:
				HideLeaderboardPanel();
				ShowHomePanel();
				break;
			case MenuFocus.Profile:
				HideProfilePanel();
				ShowHomePanel();
				break;
			case MenuFocus.Difficulty:
				HideDifficultyPanel();
				ShowHomePanel();
				break;
			case MenuFocus.Tutorial:
				HideTutorialPanel();
				ShowHomePanel();
				break;
			default:
				break;
		}
	}
}
