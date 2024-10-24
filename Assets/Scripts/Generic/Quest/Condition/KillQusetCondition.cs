using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.QuestSystem
{
    public class KillQusetCondition : IQuestCondition
    {
        // 처치해야할 적의 유형
        private string enemyType;
        // 처치해야할 총 적의 수
        private int requiredKills;
        // 현재까지 처치한 적의 수
        private int currKills;

        // 처치퀘스트 조건 초기화 생성자
        public KillQusetCondition(string enemyType, int requiredKills)
        {
            this.enemyType = enemyType;
            this.requiredKills = requiredKills;
            this.currKills = 0;
        }

        //목표 처치 수를 달성했는지 확인
        public bool IsMet() => currKills > requiredKills;
        public void Initialize() => currKills = 0;          // 처치 수를 0으로 초기화
        public float GetProgress() => (float)currKills / requiredKills;         // 현재 처치 진행도를 퍼센트로 반환
        public string GetDescription() => $"Defaet {requiredKills} {enemyType} ({currKills}/{requiredKills})";       // 퀘스트 조건 설명을 문자열로 변환

        public void EnemyKilled(string enemyType)       // 적 처치 시 호출되는 메서드
        {
            if(this.enemyType == enemyType)
            {
                currKills++;
            }
        }
    }
}

