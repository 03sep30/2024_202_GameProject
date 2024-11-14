
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MyGame.CraftingSystem
{
    [System.Serializable]
    public class Recipe
    {
        public string recipeId;                                 //레시피 고유 ID
        public IItem resultItem;                                //결과 아이템
        public int resultAmount;                                //제작 개수
        public Dictionary<int, int> requiredMaterials;          //필요 재료      <아이템 ID, 수량>
        public int requiredLevel;                               //요구 제작 레벨
        public float baseSuccessRate;                           //기본 성공 확률
        public float craftTime;                                 //제작 시간

        public Recipe(string id, IItem result, int amount)
        {
            recipeId = id;
            resultItem = result;
            resultAmount = amount;
            requiredMaterials = new Dictionary<int, int>();
            requiredLevel = 1;
            baseSuccessRate = 1;
            craftTime = 0;
        }

        public void AddRequirdMaterial(int itemId, int amount)
        {
            if (requiredMaterials.ContainsKey(itemId))
            {
                requiredMaterials[itemId] += amount;
            }
            else
            {
                requiredMaterials[itemId] = amount;
            }
        }
    }
}
