using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.CraftingSystem
{
    public class CraftingUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform recipeContent;
        [SerializeField] private GameObject recipeButtonPrefabs;
        [SerializeField] private TextMeshProUGUI selectedRecipeInfo;
        [SerializeField] private Button craftButton;

        private CraftingManager craftingManager;
        private Recipe selectedRecipe;

        // Start is called before the first frame update
        void Start()
        {
            craftingManager = CraftingManager.Instance;
            craftButton.onClick.AddListener(OnCraftButtonClick);

            RefreshRecipeList();
        }

        private void RefreshRecipeList()
        {
            foreach (Transform child in recipeContent)      // 기존 목록 제거
            {
                Destroy(child.gameObject);
            }

            foreach (var recipe  in craftingManager.GetAvaliableRecipes())      // 새 목록 생성
            {
                CreateRecipeButton(recipe);
            }
        }


        private void CreateRecipeButton(Recipe recipe)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefabs, recipeContent);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();

            text.text = recipe.resultItem.Name;
            button.onClick.AddListener(() => SelectRecipe(recipe));
        }

        private void SelectRecipe(Recipe recipe)
        {
            selectedRecipe = recipe;
            UpdateRecipeInfo();
        }

        private void UpdateRecipeInfo()
        {
            if(selectedRecipe == null)
            {
                selectedRecipeInfo.text = "Select a recipe";
                craftButton.interactable = false;
                return;
            }

            string info = $"Recipe : {selectedRecipe.resultItem.Name} \n\n Required Materials : \n ";
            foreach (var material in selectedRecipe.requiredMaterials)
            {
                info += $" - item ID {material.Key} : {material.Value} \n ";
            }

            selectedRecipeInfo.text = info;
            craftButton.interactable = true;
        }

        private void OnCraftButtonClick()
        {
            if (selectedRecipe != null)
            {
                if(craftingManager.TryCraft(selectedRecipe.recipeId))
                {
                    Debug.Log($"조합 성공 {selectedRecipe.resultItem.Name}");
                }
                else
                {
                    Debug.Log("조합 실패");
                }
            }
        }
    }
}
