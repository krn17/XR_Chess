using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceSpriteSpawner : MonoBehaviour
{
	public RectTransform whitePiecesContainer;
	public RectTransform blackPiecesContainer;

	[Header("White sprite prefabs")]
	public GameObject whitePawn;
	public GameObject whiteRook;
	public GameObject whiteBishop;
	public GameObject whiteKnight;
	public GameObject whiteQueen;
	public GameObject whiteKing;

	[Header("Black sprite prefabs")]
	public GameObject blackPawn;
	public GameObject blackRook;
	public GameObject blackBishop;
	public GameObject blackKnight;
	public GameObject blackQueen;
	public GameObject blackKing;
	
	public void SpawnPieceSpriteFromType(ChessPiece.PlayerTeam team, ChessPiece.PieceType type) {
		GameObject prefab = GetPrefabFromPiecetype(team, type);
		if(team == ChessPiece.PlayerTeam.WHITE) {
			Instantiate(prefab, whitePiecesContainer);
		}
		else {
			Instantiate(prefab, blackPiecesContainer);
		}
	}

	GameObject GetPrefabFromPiecetype(ChessPiece.PlayerTeam team, ChessPiece.PieceType type) {
		if(team == ChessPiece.PlayerTeam.WHITE) {
			switch(type) {
				case ChessPiece.PieceType.PAWN:
					return whitePawn;
				case ChessPiece.PieceType.ROOK:
					return whiteRook;
				case ChessPiece.PieceType.BISHOP:
					return whiteBishop;
				case ChessPiece.PieceType.KNIGHT:
					return whiteKnight;
				case ChessPiece.PieceType.QUEEN:
					return whiteQueen;
				case ChessPiece.PieceType.KING:
					return whiteKing;
				default:
					return whitePawn;
			}
		}
		else {
			switch(type) {
				case ChessPiece.PieceType.PAWN:
					return blackPawn;
				case ChessPiece.PieceType.ROOK:
					return blackRook;
				case ChessPiece.PieceType.BISHOP:
					return blackBishop;
				case ChessPiece.PieceType.KNIGHT:
					return blackKnight;
				case ChessPiece.PieceType.QUEEN:
					return blackQueen;
				case ChessPiece.PieceType.KING:
					return blackKing;
				default:
					return blackPawn;
			}
		}
	}
}
