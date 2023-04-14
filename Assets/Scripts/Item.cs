using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an item at scene which can be picked up or dropped
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [SerializeField] Transform anchor;
	public Transform Anchor => anchor;

	Rigidbody attachedRigidbody;
	public Rigidbody Rigidbody => attachedRigidbody;

    private void Awake()
    {
		attachedRigidbody = GetComponent<Rigidbody>();
    }

	public void OnPickedUp()
	{
		attachedRigidbody.isKinematic = true;
		attachedRigidbody.useGravity = false;
		attachedRigidbody.velocity = Vector3.zero;

		attachedRigidbody.detectCollisions = false;
	}

	public void OnDropped()
	{
		attachedRigidbody.isKinematic = false;
		attachedRigidbody.useGravity = true;

		attachedRigidbody.detectCollisions = true;
	}
}
