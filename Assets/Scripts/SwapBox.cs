using System.Collections;
using System.Collections.Generic;
using JMRSDK.InputModule;
using UnityEngine;

public class SwapBox : MonoBehaviour, ISelectClickHandler
{
	public MoveData move;
	GameManager gameManager;

	void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && move != null)
		{
			// hide the check text as it's a new turn
				UIManager.Instance.HideCheckText();
			gameManager.SwapPieces(move);
			gameManager.TempMove = move;
		}
	}
	public void OnSelectClicked(SelectClickEventData eventData)
	{
		if (eventData.PressType == JMRInteractionSourceInfo.Select && move != null)
		{
			// hide the check text as it's a new turn
				UIManager.Instance.HideCheckText();
			gameManager.SwapPieces(move);
			gameManager.TempMove = move;
		}
	}
}
