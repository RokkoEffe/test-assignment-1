using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows to pick up and throw items at scene
/// </summary>
public class ItemPicker : MonoBehaviour
{
	[Header("Pick up settings")]
	[SerializeField] float throwForce = 10f;
	[SerializeField] Transform anchor;

	[Header("Pick up animation")]
	[SerializeField] float pickUpBaseSpeed = 3f;
	[SerializeField] float pickUpIncreaseModifier = 1.1f;

	[Header("References")]
	[SerializeField] Camera raycastPickerCamera;

	// Private
	Coroutine doMoveItemToAnchorRoutine;
	Transform pickedItemParent;
	Item pickedItem;

	public void TryPickupInteractible(RayInteractible interactible)
	{
		if (interactible == null) return;
		if (interactible.TryGetComponent<Item>(out Item item))
		{
			PickupItem(item);
		}
	}

	void PickupItem(Item item)
	{
		// Save picked item
		pickedItem = item;
		pickedItemParent = item.transform.parent;

		doMoveItemToAnchorRoutine = StartCoroutine(DoMoveItemToAnchor(item));

		item.OnPickedUp();
	}

	public void DropItem(RayInteractible interactible)
	{
		if (pickedItem != null)
		{
			pickedItem.transform.parent = pickedItemParent;

			if (doMoveItemToAnchorRoutine != null)
			{
				StopCoroutine(doMoveItemToAnchorRoutine);
				doMoveItemToAnchorRoutine = null;
			}

			pickedItem.OnDropped();
			pickedItem = null;
		}
	}

	public void ThrowItem(RayInteractible interactible)
	{
		if (pickedItem != null)
		{
			var item = pickedItem;
			DropItem(interactible);
			item.Rigidbody.AddForce(anchor.forward * throwForce, ForceMode.Impulse);
		}
	}

	IEnumerator DoMoveItemToAnchor(Item item)
	{
		// Smoothly move item to the anchor and linearly increase the movement speed
		float currentSpeed = pickUpBaseSpeed;

		float distanceToAnchor = float.MaxValue;
		float rotationToAnchor = float.MaxValue;

		while (distanceToAnchor > 0.0001f || rotationToAnchor > 0.0001f)
		{
			Vector3 itemAnchorOffset = anchor.rotation * (item.Anchor.localRotation * item.Anchor.localPosition);
			Vector3 targetPosition = anchor.position - itemAnchorOffset;
			Quaternion targetRotation = anchor.rotation * item.Anchor.localRotation;

			item.transform.position = Vector3.MoveTowards(
				item.transform.position,
				targetPosition,
				currentSpeed * Time.deltaTime
			);

			distanceToAnchor = Vector3.SqrMagnitude(targetPosition - item.transform.position);

			item.transform.rotation = Quaternion.RotateTowards(
				item.transform.rotation,
				targetRotation,
				currentSpeed * Time.deltaTime * 60.0f
			);

			rotationToAnchor = Quaternion.Angle(targetRotation, item.transform.rotation);

			currentSpeed *= pickUpIncreaseModifier;

			yield return null;
		}

		// Keep item attached to the anchor via parrenting
		item.transform.parent = anchor;
		item.transform.localPosition = -1 * (item.Anchor.localRotation * item.Anchor.localPosition);
		item.transform.localRotation = item.Anchor.localRotation;
	}
}
