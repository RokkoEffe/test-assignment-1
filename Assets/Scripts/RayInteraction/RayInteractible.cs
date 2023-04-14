using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Describes an object which can be interacted with by RayInteractor
/// </summary>
public class RayInteractible : MonoBehaviour
{
	[Header("Interaction events")]
	public UnityEvent Selected;
	public UnityEvent Unselected;
	public UnityEvent Interacted;
	public UnityEvent PickedUp;
	public UnityEvent Dropped;

	[Header("Possible interaction events")]
	public UnityEvent CanInteract;
	public UnityEvent CannotInteract;

	public void Select(RayInteractor interactor)
	{
		Selected?.Invoke();
	}

	public void Unselect(RayInteractor interactor)
	{
		Unselected?.Invoke();
	}

	public void Interact(RayInteractor interactor)
	{
		Interacted?.Invoke();
	}

	public void PickUp(RayInteractor interactor)
	{
		PickedUp?.Invoke();
	}

	public void Drop(RayInteractor interactor)
	{
		Dropped?.Invoke();
	}

	public void NotifyCanInteract()
	{
		CanInteract?.Invoke();
	}

	public void NotifyCannotInteract()
	{
		CannotInteract?.Invoke();
	}
}
