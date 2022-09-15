using System.Collections;
using System.Collections.Generic;
using JMRSDK.InputModule;
using JMRSDK.Toolkit;
using UnityEngine;

public class ChessboardTransformer : MonoBehaviour, ISwipeHandler
{
	enum ObjectTransformState {
		None,
		RotateHorizontal,
		RotateVertical
	}
	public Transform chessboardHolder;
	public float objectRotateSpeed = 1.5f;
	public Vector2 yRotateRange = new Vector2(-35f, 35f);
	public Vector2 xRotateRange = new Vector2(-45f, 25f);
	public float movementSmoothingFactor = 3f;
	public JMRUIPrimaryRadioButton rotateHorizontalRadioButton;
	public JMRUIPrimaryRadioButton rotateVerticalRadioButton;
	public JMRUIPrimaryButton resetRotationButton;
	public Sprite rotateHorizontalHintImage;
	public Sprite rotateVerticalHintImage;
	
	HintManager _transformHintManager;

	private Vector3 newRotation;
	private ObjectTransformState objectTransformState = ObjectTransformState.None;

	void Start()
	{
		JMRInputManager.Instance.AddGlobalListener(gameObject);
		newRotation = chessboardHolder.eulerAngles;
		rotateHorizontalRadioButton.OnValueChanged.AddListener(OnChessboardRotateHorizontal);
		rotateVerticalRadioButton.OnValueChanged.AddListener(OnChessboardRotateVertical);
		resetRotationButton.OnClick.AddListener(ResetChessboardRotation);
		_transformHintManager = GetComponent<HintManager>();
	}

	// Update is called once per frame
	void Update()
	{
		newRotation.x = Mathf.Clamp(newRotation.x, xRotateRange.x, xRotateRange.y);
		//newRotation.y = Mathf.Clamp(newRotation.y, yRotateRange.x, yRotateRange.y);
		chessboardHolder.rotation = Quaternion.Slerp (chessboardHolder.rotation, Quaternion.Euler (newRotation.x, newRotation.y, newRotation.z), movementSmoothingFactor * Time.deltaTime);
	}

	private void OnChessboardRotateHorizontal(bool isOn)
	{
		if (isOn) {
			objectTransformState = ObjectTransformState.RotateHorizontal;
			_transformHintManager.ShowHint("Swipe Left/Right to rotate Chessboard sideways", rotateHorizontalHintImage);
		}
		else {
			// set object transform state to none if all other radio buttons are also off
			if(!rotateVerticalRadioButton.IsOn) {
				objectTransformState = ObjectTransformState.None;
				_transformHintManager.HideHint();
			}
		}
	}
	private void OnChessboardRotateVertical(bool isOn)
	{
		if (isOn) {
			objectTransformState = ObjectTransformState.RotateVertical;
			_transformHintManager.ShowHint("Swipe Up/Down to rotate Chessboard vertically", rotateVerticalHintImage);
		}
		else {
			// set object transform state to none if all other radio buttons are also off
			if(!rotateHorizontalRadioButton.IsOn) {
				objectTransformState = ObjectTransformState.None;
				_transformHintManager.HideHint();
			}
		}
	}

	public void RotateChessboardY(float value) {
		newRotation = new Vector3(newRotation.x, newRotation.y + value, newRotation.z);
	}
	public void RotateChessboardX(float value) {
		newRotation = new Vector3(newRotation.x + value, newRotation.y, newRotation.z);
	}
	public void ResetChessboardRotation() {
		newRotation = Vector3.zero;
	}

	public void OnSwipeLeft(SwipeEventData eventData, float value)
	{
		switch (objectTransformState)
		{
			case ObjectTransformState.RotateHorizontal:
				RotateChessboardY(objectRotateSpeed);
				break;
			default:
				break;
		}
	}

	public void OnSwipeRight(SwipeEventData eventData, float value)
	{
		switch (objectTransformState)
		{
			case ObjectTransformState.RotateHorizontal:
				RotateChessboardY(-objectRotateSpeed);
				break;
			default:
				break;
		}
	}

	public void OnSwipeUp(SwipeEventData eventData, float value)
	{
		switch (objectTransformState)
		{
			case ObjectTransformState.RotateVertical:
				RotateChessboardX(objectRotateSpeed);
				break;
			default:
				break;
		}
	}

	public void OnSwipeDown(SwipeEventData eventData, float value)
	{
		switch (objectTransformState)
		{
			case ObjectTransformState.RotateVertical:
				RotateChessboardX(-objectRotateSpeed);
				break;
			default:
				break;
		}
	}

	public void OnSwipeStarted(SwipeEventData eventData)
	{
		Debug.Log(gameObject.name + " Swipe Started");
	}

	public void OnSwipeUpdated(SwipeEventData eventData, Vector2 swipeData)
	{
		Debug.Log(gameObject.name + " Swipe Updated");
	}

	public void OnSwipeCompleted(SwipeEventData eventData)
	{
		Debug.Log(gameObject.name + " Swipe Complete");
	}

	public void OnSwipeCanceled(SwipeEventData eventData)
	{
		Debug.Log(gameObject.name + " Swipe Canceled");
	}
}
