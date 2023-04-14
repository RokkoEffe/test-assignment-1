using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// A button on scene with states and Clicked event
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class ImmersiveButton : MonoBehaviour
{
	[SerializeField] Color selectedColor;
	[SerializeField] Color unselectedColor;
	[SerializeField] Color clickedColor;
	[SerializeField] float clickedStateDuration = 0.1f;

	[SerializeField] MeshRenderer buttonMeshRenderer;

	public UnityEvent Clicked;

	Material material;
	Coroutine clickColorTransitionRoutine;

	private void Awake()
	{
		material = new Material(buttonMeshRenderer.material);
		buttonMeshRenderer.material = material;
	}

	public void OnSelected()
	{

		if (clickColorTransitionRoutine != null)
		{
			StopCoroutine(clickColorTransitionRoutine);
			clickColorTransitionRoutine = null;
		}

		material.color = selectedColor;
	}

	public void OnUnselected()
	{

		if (clickColorTransitionRoutine != null)
		{
			StopCoroutine(clickColorTransitionRoutine);
			clickColorTransitionRoutine = null;
		}

		material.color = unselectedColor;
	}

	public void OnClicked()
	{
		clickColorTransitionRoutine = StartCoroutine(DoClickColorTransition());
		Clicked?.Invoke();
	}

	IEnumerator DoClickColorTransition()
	{
		material.color = clickedColor;
		yield return new WaitForSeconds(clickedStateDuration);
		material.color = selectedColor;
		clickColorTransitionRoutine = null;
	}
}
