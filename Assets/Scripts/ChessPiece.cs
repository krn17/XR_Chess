using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JMRSDK.InputModule;

public class ChessPiece : MonoBehaviour, ISelectClickHandler
{
	public enum PieceType
	{
		NONE = -1,
		PAWN,
		BISHOP,
		KNIGHT,
		ROOK,
		QUEEN,
		KING,
	};

	public enum PlayerTeam
	{
		NONE = -1,
		WHITE,
		BLACK,
	};

	[SerializeField] private PieceType type = PieceType.NONE;
	[SerializeField] private PlayerTeam team = PlayerTeam.NONE;

	public PieceType Type
	{
		get
		{
			return type;
		}
	}
	public PlayerTeam Team
	{
		get
		{
			return team;
		}
	}

	private GameManager gameManager;
	private OverlayCheck overlay;
	private MeshFilter mFilter;

	public Vector3 chessPosition;
	private Vector2 moveTo;

	private MoveFunction movement = new MoveFunction(BoardManager.Instance);
	private List<MoveData> moves = new List<MoveData>();
	private int _currentMoveCount = 0;

	public List<Sprite> sprites = null;

	private bool hasMoved = false;
	public bool HasMoved
	{
		get
		{
			return hasMoved;
		}
		set
		{
			hasMoved = value;
		}
	}

	void Start()
	{
		transform.localPosition = new Vector3(chessPosition.x, transform.localPosition.y, chessPosition.y);
		moveTo = new Vector2(transform.localPosition.x, transform.localPosition.z);

		mFilter = GetComponent<MeshFilter>();
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		overlay = GameObject.FindGameObjectWithTag("ChessBoard").GetComponent<OverlayCheck>();
	}

	void Update()
	{
		transform.localPosition = new Vector3(moveTo.x, transform.localPosition.y, moveTo.y);
	}

	private void OnMouseOver()
	{
		if (!gameManager.KingDead && !gameManager.PlayerMoved)
		{
			if (Input.GetMouseButtonDown(0) && team == PlayerTeam.WHITE && gameManager.playerTurn)
			{
				moves.Clear();
				overlay.RemoveObject("Highlight");

				moves = movement.GetMoves(this, chessPosition);
				foreach (MoveData move in moves)
				{
					if (move.pieceKilled == null)
					{
						GameObject GO = Instantiate(overlay.moveHighlight, transform.parent);
						GO.transform.localPosition = new Vector3(move.secondPosition.Position.x, 0.01f, move.secondPosition.Position.y);
						GO.GetComponent<SwapBox>().move = move;
					}
					else if (move.pieceKilled != null)
					{
						GameObject GO = Instantiate(overlay.killHighlight, transform.parent);
						GO.transform.localPosition = new Vector3(move.secondPosition.Position.x, 0.01f, move.secondPosition.Position.y);
						GO.GetComponent<SwapBox>().move = move;
					}
				}
				GameObject currentGO = Instantiate(overlay.selectHighlight, transform.parent);
				currentGO.transform.localPosition = new Vector3(transform.localPosition.x, 0.01f, transform.localPosition.z);
			}
		}
	}

	public void MovePiece(Vector2 position)
	{
		moveTo = position;
	}

	public void SetType(int _type, Mesh _mesh)
	{
		type = (PieceType)_type;
		mFilter.mesh = _mesh;
	}

	public void CheckIfCheck() {

		if (team == PlayerTeam.BLACK) {
			// Preprocess all moves so that only current moves are considered
			List<MoveData> allMoves = movement.GetMoves(this, chessPosition);
			Debug.Log("All moves: "+ allMoves.Count);
			Debug.Log("Current moves: "+ _currentMoveCount);
			List<MoveData> newMoves = new List<MoveData>();
			for(int i = _currentMoveCount; i < allMoves.Count; i++) {
				newMoves.Add(allMoves[i]);
			}
			_currentMoveCount = allMoves.Count;
			// this.pos
			Debug.Log(this.name+ " AI move");
			foreach (MoveData move in newMoves) {
				// Debug.Log(this.name + " - " + this.type + ": From: " + move.firstPosition.Position + ", To: " + move.secondPosition.Position+ ", Can be killed: " + move.pieceKilled?.name);
				if(move.pieceKilled != null && move.pieceKilled.type == ChessPiece.PieceType.KING && move.pieceKilled.team == PlayerTeam.WHITE) {
					Debug.Log(move.pieceKilled.name + " can be killed, CHECKKKKKKKKKKKKKKKK");
					UIManager.Instance.ShowCheckText(true);
				}
			}
		}
	}

	public void OnSelectClicked(SelectClickEventData eventData)
	{
		if (eventData.PressType == JMRInteractionSourceInfo.Select && !gameManager.KingDead && !gameManager.PlayerMoved)
		{
			if (team == PlayerTeam.WHITE && gameManager.playerTurn)
			{
				moves.Clear();
				overlay.RemoveObject("Highlight");

				moves = movement.GetMoves(this, chessPosition);
				foreach (MoveData move in moves)
				{
					if (move.pieceKilled == null)
					{
						GameObject GO = Instantiate(overlay.moveHighlight, transform.parent);
						GO.transform.localPosition = new Vector3(move.secondPosition.Position.x, 0.01f, move.secondPosition.Position.y);
						GO.GetComponent<SwapBox>().move = move;
					}
					else if (move.pieceKilled != null)
					{
						GameObject GO = Instantiate(overlay.killHighlight, transform.parent);
						GO.transform.localPosition = new Vector3(move.secondPosition.Position.x, 0.01f, move.secondPosition.Position.y);
						GO.GetComponent<SwapBox>().move = move;
					}
				}
				GameObject currentGO = Instantiate(overlay.selectHighlight, transform.parent);
				currentGO.transform.localPosition = new Vector3(transform.localPosition.x, 0.01f, transform.localPosition.z);

				
			}
			//CheckIfCheck();
		}
	}
}
