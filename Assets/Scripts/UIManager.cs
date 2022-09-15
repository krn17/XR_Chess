using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JMRSDK.Toolkit;
using DG.Tweening;
using JMRSDK.InputModule;

public class UIManager : MonoBehaviour, IBackHandler, IHomeHandler
{
	GameManager gameManager;
	MiniMax minMax;

	[Header("===Texts===")]
	public TextMeshProUGUI TurnText;
	public TextMeshProUGUI WinnerText;
	public TextMeshProUGUI TurnCountText;
	public TextMeshProUGUI DifficultyText;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI checkText;
	public Color enemyCheckCol;
	public Color playerCheckCol;

	[Header("===Buttons===")]
	public JMRUIPrimaryButton headerPauseBtn;

	[Header("===Game Over Screen===")]
	public RectTransform gameOverPanel;
	public JMRUIPrimaryButton restartSceneBtn;
	public JMRUIPrimaryButton goHomeBtn;
	public TextMeshProUGUI gameOverTurnCountText;
	public TextMeshProUGUI gameOverTimerText;
	public TextMeshProUGUI gameOverScoreText;
	public GameObject[] disableOnGameOver;

	[Header("===Pause Menu===")]
	public RectTransform pauseMenuPanel;
	public JMRUIPrimaryButton resumeBtn;
	public JMRUIPrimaryButton pausedGoHomeBtn;
	public TextMeshProUGUI pausedTurnCountText;
	public TextMeshProUGUI pausedTimerText;
	private bool _pauseMenuOpen = false;
	private SceneLoader _sceneLoader;


	public static UIManager Instance = null;

	private float startTime;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(this);
		}

		gameManager = GameManager.Instance;
		
	}

	public void Start()
	{
		_sceneLoader = GetComponent<SceneLoader>();
		minMax = gameManager.MinMax;
		
		headerPauseBtn.OnClick.AddListener(() => {ShowPauseMenuPanel();});
		restartSceneBtn.OnClick.AddListener(() => {ChangeScene("Sandbox");});
		goHomeBtn.OnClick.AddListener(() => {ChangeScene("MenuScreen");});
		resumeBtn.OnClick.AddListener(() => HidePauseMenuPanel());
		pausedGoHomeBtn.OnClick.AddListener(() => {
			Time.timeScale = 1f;
			ChangeScene("MenuScreen");
			});

		ChangeDepth();
		HideCheckText();
		startTime = Time.time;
	}

	private void Update() {
		UpdateTimer();
	}

	string UpdateTimer() {
		if(gameManager.KingDead)
			return timerText.text;

		float t = Time.time - startTime;
		string min = ((int)t/60).ToString();
		string sec = (t%60).ToString("f0");
		// string format
		if(min.Length == 1)
			min = "0"+min;
		if(sec.Length == 1)
			sec = "0"+sec;
		timerText.text = min + ":" + sec;

		return min + ":" + sec;
	}

	public void PlayerTurnText(bool _playerTurn)
	{
		if (_playerTurn)
		{
			TurnText.text = "White Turn";
		}
		else if (!_playerTurn)
		{
			TurnText.text = "Black Turn";
		}
	}

	public void GameRestart(bool _kingDead, bool _isWhiteWin)
	{
		if (_kingDead)
		{
			// RestartButton.gameObject.SetActive(true);
			if (_isWhiteWin)
			{
				WinnerText.text = "White Wins!";
			}
			else if (!_isWhiteWin)
			{
				WinnerText.text = "Black Wins!";
			}

			ShowGameOverPanel(_isWhiteWin);
		}
		else
		{
			return;
		}
	}

	public void TurnCount(int _turnCount)
	{
		TurnCountText.text =_turnCount.ToString();
		// update turn count of gameover screen as well
		gameOverTurnCountText.text = _turnCount.ToString();
		// update turn count of pausemenu screen as well
		pausedTurnCountText.text = _turnCount.ToString();
	}

	public void ResetLevel(int _level)
	{
		SceneManager.LoadScene(_level);
	}

	// public void CheckMoved(bool _playerMoved, bool _kingDead)
	// {
	//     if (_playerMoved && !_kingDead)
	//     {
	//         UndoButton.gameObject.SetActive(true);
	//         EndTurnButton.gameObject.SetActive(true);
	//     }
	//     else if (!_playerMoved)
	//     {
	//         UndoButton.gameObject.SetActive(false);
	//         EndTurnButton.gameObject.SetActive(false);
	//     }
	// }

	public void ChangeDepth()
	{
		int difficulty = PlayerPrefs.GetInt("game_difficulty", 3);
		minMax.MaxDepth = difficulty;

		DepthNumber();

		Debug.Log("MaxDepth : " + minMax.MaxDepth);
	}

	public void DepthNumber()
	{
		DifficultyText.text = "Difficulty: " + minMax.MaxDepth.ToString();
	}
	public void ShowGameOverPanel(bool _isWhiteWin) {
		gameOverTimerText.text = UpdateTimer();
		// calculate and show score
		int score = gameManager.CalculateScore(startTime, _isWhiteWin);
		if(score > SaveManager.Load().saveObject.highscore) {
			gameOverScoreText.text = score + " (Highscore!)";
			SaveManager.SaveHighscore(score);
		}
		else {
			gameOverScoreText.text = score + "";
		}
		// update highscore to leaderboard
		try {
			SaveData saveData = SaveManager.Load();
			if(saveData.saveObject.highscore > 0) {
				HighScores.UploadScore(saveData.saveObject.playerName, saveData.saveObject.highscore);
			}
		}
		catch(System.Exception) {
			Debug.Log("Error uploading to leaderboard");
		}
		// disable on game over
		for(int i = 0; i< disableOnGameOver.Length; i++) {
			disableOnGameOver[i].SetActive(false);
		}
		gameOverPanel.gameObject.SetActive(true);
		gameOverPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f).SetUpdate(true);
		gameOverPanel.DOShakeScale(0.3f,0.2f, 20, 90f).SetUpdate(true);
	}

	public void ShowPauseMenuPanel() {
		_pauseMenuOpen = true;
		Time.timeScale = 0f;
		pausedTimerText.text = UpdateTimer();
		// disable on pause menu
		for(int i = 0; i< disableOnGameOver.Length; i++) {
			disableOnGameOver[i].SetActive(false);
		}
		pauseMenuPanel.gameObject.SetActive(true);
		pauseMenuPanel.GetComponent<CanvasGroup>().DOFade(1f, 0.15f).SetUpdate(true);
		pauseMenuPanel.DOShakeScale(0.3f,0.2f, 20, 90f).SetUpdate(true);
	}
	public void HidePauseMenuPanel() {
		_pauseMenuOpen = false;
		Time.timeScale = 1f;
		// enable on resume
		for(int i = 0; i< disableOnGameOver.Length; i++) {
			disableOnGameOver[i].SetActive(true);
		}
		pauseMenuPanel.GetComponent<CanvasGroup>().alpha = 0f;
		pauseMenuPanel.gameObject.SetActive(false);
	}

	public void ShowCheckText(bool isEmenyCheck) {
		Debug.Log("SHOWING CHECK TEXT");
		checkText.gameObject.SetActive(true);
		checkText.color = isEmenyCheck ? enemyCheckCol : playerCheckCol;
	}
	public void HideCheckText() {
		checkText.gameObject.SetActive(false);
		// Color col = checkText.color;
		// col.a = 0f;
		// checkText.color = col;
	}
	public void ChangeScene(string name) {
		_sceneLoader.LoadSceneAsync(name);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void OnBackAction()
	{
		if(!_pauseMenuOpen)
			ShowPauseMenuPanel();
		else
			HidePauseMenuPanel();
	}

	public void OnHomeAction()
	{
		if(!_pauseMenuOpen)
			ShowPauseMenuPanel();
	}
}
