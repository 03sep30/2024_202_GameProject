using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.QuestSystem
{
    public class CollectionQusetCondition : IQuestCondition         //아이템을 수집하는 퀘스트 조건을 정의하는 퀘스트
    {
        private string itemId;      //수집해야할 아이템 ID
        private int requiredAmont;  //수집해야할 아이템 개수
        private int currentAmont;   //현재까지 수집한 아이템 개수

        public CollectionQusetCondition(string itemId, int requiredAmont)       //생성자에게 아잍템 ID와 필요한 개수를 설정
        {
            this.itemId = itemId;
            this.requiredAmont = requiredAmont;
            this.currentAmont = 0;
        }

        public bool IsMet() => currentAmont > requiredAmont;        //퀘스트 조건이 충족되었는지 여부 확인
        public void Initialize() => currentAmont = 0;               // 조건을 초기화 하여 수집량 0
        public float GetProgress() => (float)currentAmont / requiredAmont;      // 현재 진행 상황을 0에서 1사이의 값으로 반환
        public string GetDescription() => $"Defaet {requiredAmont} {itemId} ({currentAmont}/{requiredAmont})";       // 퀘스트 조건 설명을 문자열로 변환
        
        public void ItemCollected(string itemId)
        {
            if(this.itemId == itemId)
            {
                currentAmont = 0;
            }
        }
    }
}
