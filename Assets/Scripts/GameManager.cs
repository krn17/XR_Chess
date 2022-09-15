using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessAR;
using System.Threading;

public class GameManager : MonoBehaviour
{
	private MiniMax minMax = new MiniMax();
	UIManager uiManager;
	BoardManager board;
	OverlayCheck overlay;

	public bool playerTurn = true;
	bool playerMoved = false;
	public bool PlayerMoved
	{
		get
		{
			return playerMoved;
		}
		set
		{
			playerMoved = value;
		}
	}
	bool kingDead = false;
	public bool KingDead
	{
		get
		{
			return kingDead;
		}
		set
		{
			kingDead = value;
		}
	}
	bool isWhiteWin = false;
	public bool IsWhiteWin
	{
		get
		{
			return isWhiteWin;
		}
		set
		{
			isWhiteWin = value;
		}
	}
	float timer = 0.0f;
	int turnCount = 0;
	public int TurnCount
	{
		get
		{
			return turnCount;
		}
		set
		{
			turnCount = value;
		}
	}

	MoveData tempMove = null;
	public MoveData TempMove
	{
		get
		{
			return tempMove;
		}
		set
		{
			tempMove = value;
		}
	}

	public MiniMax MinMax
	{
		get
		{
			return minMax;
		}

		set
		{
			minMax = value;
		}
	}

	[Header("===Queen Meshes===")]
	public Mesh queen_White;
	public Mesh queen_Black;
	[Header("===Pawn Meshes===")]
	public Mesh pawn_White;
	public Mesh pawn_Black;

	[Header("===Last Tag parent===")]
	public Transform lastTagParent;
	private bool gameOverScreenShown = false;
	private PieceSpriteSpawner _pieceSpriteSpawner;

	public static GameManager Instance;
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
		//minMax = GetComponent<MiniMax>();
	}

	void Start()
	{
		overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
		board = BoardManager.Instance;
		uiManager = UIManager.Instance;
		_pieceSpriteSpawner = GetComponent<PieceSpriteSpawner>();

		board.SetupBoard();
		// uiManager.CheckMoved(playerMoved);
		uiManager.TurnCount(turnCount);
		uiManager.PlayerTurnText(playerTurn);
	}
	bool aiMoveDone = true;
	private void Update()
	{
		if (kingDead && !gameOverScreenShown)
		{
			gameOverScreenShown = true;
			//Make a restart button.
			uiManager.GameRestart(kingDead, isWhiteWin);
		}
		else if (!kingDead && aiMoveDone)
		{
			if (!playerTurn && timer >= 1.0f)
			{
				timer = 0.0f;
				Thread thread = minMax.GetMove();
				StartCoroutine(WaitForThreadFinish(thread));
				aiMoveDone = false;
				// MoveData move = MinMax.GetMove();
				// DoAIMove(move);
				//playerTurn = !playerTurn;
				//uiManager.PlayerTurnText(playerTurn);
			}
			else if (!playerTurn)
			{
				timer += Time.deltaTime;
			}
		}
	}

	IEnumerator WaitForThreadFinish(Thread thread) {
		while(thread.IsAlive) {
			yield return null;
		}
		DoAIMove(minMax.bestMove);
		aiMoveDone = true;
	}

	public int CalculateScore(float startTime, bool isPlayerWin) {
		int baseScore = 1000 * minMax.MaxDepth;
		baseScore -= (int)(Time.time - startTime);
		if(baseScore <= 100)
			baseScore = 100;
		Debug.Log("Time passed: " + (int)(Time.time - startTime));
		return isPlayerWin ? baseScore : 0;
	}

	void DoAIMove(MoveData move)
	{
		TileData firstPosition = move.firstPosition;
		TileData secondPosition = move.secondPosition;

		SwapPieces(move);

		//if (!kingDead)
		//{
		//    playerTurn = !playerTurn;
		//    turnCount++;

		//    uiManager.TurnCount(turnCount);
		//    uiManager.PlayerTurnText(playerTurn);
		//}
	}

	public void SwapPieces(MoveData move)
	{
		overlay.RemoveObject("Highlight");
		overlay.RemoveObject("LastTag");

		TileData firstTile = move.firstPosition;
		TileData secondTile = move.secondPosition;

		LastMoveTag(move);

		firstTile.CurrentPiece.MovePiece(new Vector2(secondTile.Position.x, secondTile.Position.y));

		ConvertPawn(firstTile, move);

		CheckDeath(secondTile);

		secondTile.CurrentPiece = move.pieceMoved;
		firstTile.CurrentPiece = null;
		secondTile.CurrentPiece.chessPosition = secondTile.Position;
		secondTile.CurrentPiece.HasMoved = true;

		// Check if the latest AI move imposes a Check
		secondTile.CurrentPiece.CheckIfCheck();

		// play turn sound
		AudioManager.instance.PlaySound2D("chess_turn");

		UpdateTurn();

		//if (playerTurn)
		//{
		//    playerMoved = true;
		//}
		//uiManager.CheckMoved(playerMoved, kingDead);
	}

	// void CheckIfCheck(ChessPiece piece) {
	// 	if(piece.Team == ChessPiece.PlayerTeam.BLACK) {
	// 		//Debug.Log("Black piece name: " + piece.name);
	// 		piece.getmoves
	// 	}
	// }

	public void UndoMove()
	{
		overlay.RemoveObject("LastTag");

		TileData firstTile = tempMove.firstPosition;
		TileData secondTile = tempMove.secondPosition;

		secondTile.CurrentPiece.MovePiece(new Vector2(firstTile.Position.x, firstTile.Position.y));

		//ReturnPawn(firstTile, tempMove);

		//SpriteRenderer sRend = secondTile.CurrentPiece.GetComponent<SpriteRenderer>();
		//sRend.enabled = true;
		//secondTile.CurrentPiece.gameObject.SetActive(true);

		firstTile.CurrentPiece = tempMove.pieceMoved;
		secondTile.CurrentPiece = null;
		firstTile.CurrentPiece.chessPosition = firstTile.Position;
		firstTile.CurrentPiece.HasMoved = false;

		playerMoved = false;

		//uiManager.CheckMoved(playerMoved, kingDead);
	}

	private void UpdateTurn()
	{
		if (!kingDead)
		{
			playerTurn = !playerTurn;
			turnCount++;

			uiManager.TurnCount(turnCount);
			uiManager.PlayerTurnText(playerTurn);
		}
	}

	void CheckDeath(TileData _secondTile)
	{
		if (_secondTile.CurrentPiece != null)
		{
			if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)
			{
				kingDead = true;
				if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
				{
					isWhiteWin = true;
				}
				else if (_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
				{
					isWhiteWin = false;
				}
			}
			//SpriteRenderer sRend = _secondTile.CurrentPiece.GetComponent<SpriteRenderer>();
			//sRend.enabled = false;
			//_secondTile.CurrentPiece.gameObject.SetActive(false);
			if(_secondTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK) {
				Debug.Log(_secondTile.CurrentPiece.Type + "GOT DESTROYED BRUH");
			}
			else {
				Debug.Log(_secondTile.CurrentPiece.Type + "DESTROYED MEEEEEEEEEE");

			}
			_pieceSpriteSpawner.SpawnPieceSpriteFromType(_secondTile.CurrentPiece.Team, _secondTile.CurrentPiece.Type);
			Destroy(_secondTile.CurrentPiece.gameObject);
		}
	}

	//Special rule, pawn becomes queen.
	void ConvertPawn(TileData _firstTile, MoveData _move)
	{
		if (_firstTile.CurrentPiece.Type == ChessPiece.PieceType.PAWN)
		{
			if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
			{
				if (_move.secondPosition.Position.y == 7)
				{
					_firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_White);
				}
			}
			else if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
			{
				if (_move.secondPosition.Position.y == 0)
				{
					_firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.QUEEN, queen_Black);
				}
			}
		}
	}
	//Inverse special rule for undo method. (requires reference of past tile)
	void ReturnPawn(TileData _firstTile, MoveData _move)
	{
		if (_firstTile.CurrentPiece.Type == ChessPiece.PieceType.PAWN)
		{
			if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.WHITE)
			{
				if (_move.secondPosition.Position.y == 7)
				{
					_firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.PAWN, pawn_White);
				}
			}
			else if (_firstTile.CurrentPiece.Team == ChessPiece.PlayerTeam.BLACK)
			{
				if (_move.secondPosition.Position.y == 7)
				{
					_firstTile.CurrentPiece.SetType((int)ChessPiece.PieceType.PAWN, pawn_Black);
				}
			}
		}
	}

	void LastMoveTag(MoveData move)
	{
		GameObject GOfrom = Instantiate(overlay.lastHighlight, lastTagParent);
		GOfrom.transform.localPosition = new Vector3(move.firstPosition.Position.x, 0.01f,  move.firstPosition.Position.y);

		GameObject GOto = Instantiate(overlay.lastHighlight, lastTagParent);
		GOto.transform.localPosition = new Vector3(move.secondPosition.Position.x, 0.01f, move.secondPosition.Position.y);
	}

	public void PlayerEndTurn()
	{
		//playerMoved = false;
		playerTurn = !playerTurn;
		turnCount++;

		//uiManager.CheckMoved(playerMoved, kingDead);
		uiManager.TurnCount(turnCount);
		uiManager.PlayerTurnText(playerTurn);
	}
}
