using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.QuestSystem
{
    public class CollectionQusetCondition : IQuestCondition         //�������� �����ϴ� ����Ʈ ������ �����ϴ� ����Ʈ
    {
        private string itemId;      //�����ؾ��� ������ ID
        private int requiredAmont;  //�����ؾ��� ������ ����
        private int currentAmont;   //������� ������ ������ ����

        public CollectionQusetCondition(string itemId, int requiredAmont)       //�����ڿ��� �Ɵ��� ID�� �ʿ��� ������ ����
        {
            this.itemId = itemId;
            this.requiredAmont = requiredAmont;
            this.currentAmont = 0;
        }

        public bool IsMet() => currentAmont > requiredAmont;        //����Ʈ ������ �����Ǿ����� ���� Ȯ��
        public void Initialize() => currentAmont = 0;               // ������ �ʱ�ȭ �Ͽ� ������ 0
        public float GetProgress() => (float)currentAmont / requiredAmont;      // ���� ���� ��Ȳ�� 0���� 1������ ������ ��ȯ
        public string GetDescription() => $"Defaet {requiredAmont} {itemId} ({currentAmont}/{requiredAmont})";       // ����Ʈ ���� ������ ���ڿ��� ��ȯ
        
        public void ItemCollected(string itemId)
        {
            if(this.itemId == itemId)
            {
                currentAmont = 0;
            }
        }
    }
}
