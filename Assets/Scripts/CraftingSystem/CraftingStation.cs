using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;

public class CraftingStation : MonoBehaviour
{
	[Header("Recipes folder in Resources")]
	[SerializeField] string recipesFolderName = "Crafting Recipes";

	[Header("References")]
    [SerializeField] TextMeshPro _ingredientsList = null;
    [SerializeField] TextMeshPro _result = null;
	[SerializeField] ItemVolumeTrigger ingredientsVolumeTrigger;
	[SerializeField] Transform craftedItemSpawnPoint;

	// Private
	List<CraftingRecipe> allowedRecipes = new List<CraftingRecipe>();
	List<CraftingItem> currentIngredients = new List<CraftingItem>();
	CraftingRecipe matchedRecipe;

	private void Start()
	{
		// Fetch available recipes from Resources folder
		allowedRecipes = Resources.LoadAll<CraftingRecipe>(recipesFolderName).ToList();
	}

	void OnEnable()
	{
		ingredientsVolumeTrigger.ItemRegistered.AddListener(AddIngredient);
		ingredientsVolumeTrigger.ItemUnregistered.AddListener(RemoveIngredient);
	}

	void OnDisable()
	{
		ingredientsVolumeTrigger.ItemRegistered.AddListener(AddIngredient);
		ingredientsVolumeTrigger.ItemUnregistered.AddListener(RemoveIngredient);
	}

    void Awake()
    {
        RefreshStatus();
    }

    void RefreshStatus()
    {
		if (currentIngredients.Count > 0)
		{
			_ingredientsList.text = String.Join("\n", currentIngredients.Select(ingredient => ingredient.ItemName));

			TryMatchPossibleRecipe();
		}
		else
		{
			_ingredientsList.text = "Waiting for ingredients...";
			_result.text = "";
			matchedRecipe = null;		
		}
    }

	void AddIngredient(CraftingItem item)
	{
		currentIngredients.Add(item);
		RefreshStatus();
	}

	void RemoveIngredient(CraftingItem item)
	{
		currentIngredients.Remove(item);
		RefreshStatus();
	}

	void TryMatchPossibleRecipe()
	{
		string currentIngredientsHash = CraftingRecipe.IngredientsToHash(currentIngredients);

		CraftingRecipe matchedRecipe = allowedRecipes
			.FirstOrDefault(recipe => recipe.IngredientsHash == currentIngredientsHash);
		
		if (matchedRecipe == null)
		{
			_result.text = "Wrong recipe!";
		}
		else
		{
			_result.text = matchedRecipe.result.ItemName;
		}

		this.matchedRecipe = matchedRecipe;
	}

	public void TryCraftRecipe()
	{
		if (matchedRecipe != null)
		{
			Instantiate(matchedRecipe.result, craftedItemSpawnPoint.position, Quaternion.identity);
			foreach (CraftingItem ingredient in currentIngredients)
			{
				Destroy(ingredient.gameObject);
			}
			currentIngredients.Clear();

			RefreshStatus();
		}
	}
}
