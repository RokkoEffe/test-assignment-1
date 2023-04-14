using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that registers all Item objects that are within the attacher Trigger volume
/// </summary>
[RequireComponent(typeof(Collider))]
public class ItemVolumeTrigger : MonoBehaviour
{
	public UnityEvent<CraftingItem> ItemRegistered;
	public UnityEvent<CraftingItem> ItemUnregistered;

	void OnTriggerEnter(Collider other)
	{
		CraftingItem item = other.GetComponent<CraftingItem>();
		ItemRegistered?.Invoke(item);
	}

	void OnTriggerExit(Collider other)
	{
		CraftingItem item = other.GetComponent<CraftingItem>();
		ItemUnregistered?.Invoke(item);
	}
}
