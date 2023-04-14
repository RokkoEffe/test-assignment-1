using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls SpriteRenderer indicator visibility
/// </summary>
public class InteractibleIndicator : MonoBehaviour
{
	[SerializeField] SpriteRenderer inRangeMark;
	[SerializeField] SpriteRenderer canBeInteractedMark;

	private void Start()
	{
		inRangeMark.enabled = false;
		canBeInteractedMark.enabled = false;
	}

	public void SetInRange()
	{
		inRangeMark.enabled = true;
	}

	public void SetNotInRange()
	{
		inRangeMark.enabled = false;
	}

	public void SetCanInteract()
	{
		canBeInteractedMark.enabled = true;
	}

	public void SetCannotInteract()
	{
		canBeInteractedMark.enabled = false;
	}
}
