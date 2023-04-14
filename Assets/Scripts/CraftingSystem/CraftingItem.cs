using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that describes an item which can participate in crafting process as an ingredient or result
/// </summary>
public class CraftingItem : MonoBehaviour
{
	// Unique ID is generated each time a component is attached to GameObject (Prefab)
	// This ID persists among all instances of prefab
	[SerializeField] [HideInInspector] 
	string id = Guid.NewGuid().ToString();
	public string ID => id;

	[SerializeField] string itemName;
	[HideInInspector] public string ItemName => itemName;
}
