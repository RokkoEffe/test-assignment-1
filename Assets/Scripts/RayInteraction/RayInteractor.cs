using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Allows to interact with RayInteractibles
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class RayInteractor : MonoBehaviour
{
	[Header("Raycast")]
	[SerializeField] Camera raycastCamera;
	[SerializeField] float rayLength;
	[SerializeField] LayerMask interactibleLayers;

	[Header("Input actions")]
	[SerializeField] InputActionProperty pickUpAction;
	[SerializeField] InputActionProperty interactAction;

	[Header("Events")]
	public UnityEvent<RayInteractible> Selected;
	public UnityEvent<RayInteractible> Unselected;
	public UnityEvent<RayInteractible> PickedUp;
	public UnityEvent<RayInteractible> Dropped;
	public UnityEvent<RayInteractible> Interacted;

	// Private

	// Interactibles
	GameObject selectedObject;
	RayInteractible selectedInteractible;
	RayInteractible pickedUpInteractible;

	// Interact notification
	SphereCollider interactionRadiusSphere;

	private void Awake() => interactionRadiusSphere = GetComponent<SphereCollider>();

	private void OnValidate()
	{
		if (interactionRadiusSphere == null) interactionRadiusSphere = GetComponent<SphereCollider>();

		// Interaction Sphere radius should always match the ray length and stay in trigger mode
		interactionRadiusSphere.radius = rayLength;
		interactionRadiusSphere.isTrigger = true;
	}

	void OnEnable()
	{
		pickUpAction.action.performed += OnPickUpAction;
		pickUpAction.action.canceled += OnDropAction;
		interactAction.action.performed += OnInteractedAction;
	}

	void OnDisable()
	{
		pickUpAction.action.performed -= OnPickUpAction;
		pickUpAction.action.canceled += OnDropAction;
		interactAction.action.performed -= OnInteractedAction;
	}

	void Update()
	{
		// Constantly raycasting to look for interactibles
		RaycastForInteractibles();

		transform.position = raycastCamera.transform.position;
	}

	void RaycastForInteractibles()
	{
		Ray ray = raycastCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

		// Interaction sphere should be centered in ray origin to correctly detect possible interactibles
		interactionRadiusSphere.center = ray.origin - transform.position;

		// RayInteractor should align with ray origin to detect interactibles in range
		transform.position = ray.origin;
		if (Physics.Raycast(ray, out RaycastHit hit, rayLength, interactibleLayers))
		{
			// As object is in interactibleLayers, assume that it is interactible

			// Fire events only when object is raycasted for the first time
			if (hit.transform.gameObject == selectedObject) return;
			else
			{
				// Clear previously selected interactible
				selectedInteractible?.Unselect(this);

				// Select new interactible
				selectedObject = hit.transform.gameObject;
				selectedInteractible = hit.transform.GetComponent<RayInteractible>();
				selectedInteractible.Select(this);
				Selected?.Invoke(selectedInteractible);
			}
		}
		else if (selectedObject != null)
		{
			// Unselect item
			selectedInteractible.Unselect(this);
			Unselected?.Invoke(selectedInteractible);
			selectedObject = null;
			selectedInteractible = null;
		}
	}

	void OnPickUpAction(InputAction.CallbackContext context)
	{
		selectedInteractible?.PickUp(this);
		pickedUpInteractible = selectedInteractible;
		PickedUp?.Invoke(pickedUpInteractible);
	}

	void OnDropAction(InputAction.CallbackContext context)
	{
		pickedUpInteractible?.Drop(this);
		Dropped?.Invoke(pickedUpInteractible);
		pickedUpInteractible = null;
	}

	void OnInteractedAction(InputAction.CallbackContext context)
	{
		// When interacting, priority is given to the picked up item
		if (pickedUpInteractible != null)
		{
			pickedUpInteractible.Interact(this);
			Interacted?.Invoke(pickedUpInteractible);
		}
		else
		{
			selectedInteractible?.Interact(this);
			Interacted?.Invoke(selectedInteractible);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// Notify that interactible in radius can be interacted by ray
		if (other.TryGetComponent<RayInteractible>(out RayInteractible interactible))
		{
			interactible.NotifyCanInteract();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		// Notify that interactible in radius can NO longer be interacted by ray
		if (other.TryGetComponent<RayInteractible>(out RayInteractible interactible))
		{
			interactible.NotifyCannotInteract();
		}
	}
}
