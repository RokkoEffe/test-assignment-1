using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

/// <summary>
/// A crafting recipe for the CraftingStation
/// </summary>
[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting System/Crafting Recipe", order = 0)]
[ExecuteAlways]
public class CraftingRecipe : ScriptableObject
{
	public List<CraftingItem> ingredients = new List<CraftingItem>();
	public CraftingItem result;

	// Ingredients hash is generated automatically in Editor and serialized
	[SerializeField] [HideInInspector] string ingredientsHash;
	public string IngredientsHash => ingredientsHash;

	private void OnValidate()
	{
		// Re-calculate ingredients hash when user changes recipe in Inspector
		if (ingredients.Count <= 0 || ingredients.Any(ingredient => ingredient == null))
		{
			ingredientsHash = "0";
		}
		else ingredientsHash = IngredientsToHash(ingredients);
	}

	/// <summary>
	/// Converts a list of ingredients into a string hash which describes a unique recipe
	/// </summary>
	/// <param name="list"></param>
	/// <typeparam name="CraftingItem"></typeparam>
	/// <returns></returns>
	public static string IngredientsToHash(List<CraftingItem> list)
    {
        var elementCounts = new Dictionary<string, int>();
        foreach (var element in list)
        {
            if (elementCounts.ContainsKey(element.ID)) elementCounts[element.ID]++;
            else elementCounts[element.ID] = 1;
        }

        var stringBuilder = new StringBuilder();
        foreach (var kvp in elementCounts.OrderBy(x => x.Key.GetHashCode()))
        {
            stringBuilder.Append($"{kvp.Key}:{kvp.Value},");
        }
        stringBuilder.Length--;

        var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
        return System.Convert.ToBase64String(hashBytes);
    }
}
