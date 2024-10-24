using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.QuestSystem
{
    public class ExperienceReward : IQuestReward        // 경험치 보상을 구현하는 클래스
    {
        private int experienceAmount;                   // 보상으로 지급할 경험치량
        public ExperienceReward(int amount)             // 경험치 보상 초기화 생성자
        {
            this.experienceAmount = amount;
        }

        public void Grant(GameObject player)
        {
            // TODO : 실제 경험치 지급 로직 구현
            Debug.Log($"Granted {experienceAmount} experience");
        }

        public string GetDescription() => $"{experienceAmount} Experience Points";      //보상 내용을 문자열로 반환
    }

}
