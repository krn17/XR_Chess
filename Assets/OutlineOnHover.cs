using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JMRSDK;
using JMRSDK.InputModule;
using QuickOutline;

public class OutlineOnHover : MonoBehaviour, IFocusable
{
	Outline outline;
	public float outlineWidth = 2f;

	// Start is called before the first frame update
	void Start()
	{
		outline = GetComponent<Outline>();
	}


	public void OnFocusEnter()
	{
		outline.OutlineWidth = outlineWidth;
	}

	public void OnFocusExit()
	{
		outline.OutlineWidth = 0f;
	}
}
