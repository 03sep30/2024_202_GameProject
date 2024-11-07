using MyGame.QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();
    private Dictionary<string, Quest> activeQuests = new Dictionary<string, Quest>();
    private Dictionary<string, Quest> completedQuests = new Dictionary<string, Quest>();

    public event Action<Quest>  OnQuestStarted;
    public event Action<Quest> OnQuestCompleted;
    public event Action<Quest> OnQuestFailed;

    public void Start()
    {
        InitializeQuests();
    }


    // 기본 퀘스트들을 생성하고 등록하는 메서드
    private void InitializeQuests()
    {
        // 쥐 사냥 퀘스트 생성 예시
        var ratHuntQuest = new Quest("Q001", "Rat Problem", "Clear the basement of rate", QuestType.Kill, 1);
        ratHuntQuest.Addcondition(new KillQusetCondition("Rat", 5));
        ratHuntQuest.AddReward(new ExperienceReward(100));
        ratHuntQuest.AddReward(new ItemReward("Gold", 50));

        // 약초 수집 퀘스트
        var herbQuest = new Quest("Q002", "Herb Collection", "Collect herbs for the healer", QuestType.Collection, 1);
        herbQuest.Addcondition(new CollectionQusetCondition("Herb", 3));
        herbQuest.AddReward(new ExperienceReward(50));

        // 퀘스트 매니저에 퀘스트 추가
        allQuests.Add(ratHuntQuest.Id, ratHuntQuest);
        allQuests.Add(herbQuest.Id, herbQuest);

        // 테스트 위해서 바로 시작 (StartQuest 함수)
        StartQuest("Q001");
        StartQuest("Q002");

    }

    public bool CanStartQuest(string questId)       // 특정 퀘스트를 시작할 수 있는지 검사하는 메서드
    {
        if(!allQuests.TryGetValue(questId, out Quest quest)) return false;
        if(activeQuests.ContainsKey(questId)) return true;
        if(completedQuests.ContainsKey(questId)) return true;

        // 선행 퀘스트 완료 여부 확인
        foreach (var perrequistiteId in quest.GetType().GetField("prerequisiteQuestIds")?.GetValue(quest) as List<string> ?? new List<string>())
        {
            if(!completedQuests.ContainsKey(perrequistiteId)) return false;
        }

        //Type questType = quest.GetType();                                                         // Quest 객체의 타입을 가져온다.           
        //FieldInfo perrequisiteIdsField = GetField("prerequisiteQuestIds");                        // Quest Type 에서 필드를 검색
        //object perrequisiteIdsValue = perrequisiteIdsField.GetValue(quest);                       // 필드 값을 가져온다.
        //List<string> prerequisiteQuestIds = perrequisiteIdsValue as List<string>();               // List로 변환하고
        //prerequisiteQuestIds = prerequisiteQuestIds ?? new List<string>();                        // Null 값일 경우에는 빈 List를 사용한다.
        // ?? Null 병합 연산자 -> 왼쪽 값이 Null인지 확인하여 Null일 경우 오른쪽으로 반환

        return true;

    }

    // 퀘스트를 시작하는 메서드
     public void StartQuest(string questId)
     {
        if(!CanStartQuest(questId)) return;
       
         var quest = allQuests[questId];
         quest.Start();
         activeQuests.Add(questId, quest);
         OnQuestStarted?.Invoke(quest);
       
     }

    // 퀘스트 진행 상황을 업데이트 하는 메서드
    public void UpdateQuestProgreses(string questId)
    {
        if(!activeQuests.TryGetValue(questId,out Quest quest)) return;
        
        if(quest.CheckCompletion())
        {
            CompleteQuest(questId);
        }
    }

    // 퀘스트를 완료 처리 하는 메서드
    private void CompleteQuest(string questId)
    {
        if (!activeQuests.TryGetValue(questId, out Quest quest)) return;

        // 플레이어 찾기 실패해도 퀘스트는 완료
        var player = GameObject.FindGameObjectWithTag("Player");
        quest.Complete(player);

        activeQuests.Remove(questId);
        completedQuests.Add(questId, quest);
        OnQuestCompleted?.Invoke(quest);

        Debug.Log($"Quest completed : { quest.Title}");
    }

    // 시작 가능한 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetAvailableQuests()
    {
        return allQuests.Values.Where(q => CanStartQuest(q.Id)).ToList();
    }

    // 현재 진행 중인 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetActiveQuest()
    {
        return activeQuests.Values.ToList();
    }

    // 완료된 퀘스트 목록을 반환하는 메서드
    public List<Quest> GetCompletedQuest()
    {
        return completedQuests.Values.ToList();
    }


    // 적 처치 시 호출되는 이벤트 핸들러
    public void OnEnemyKilled(string enemytype)
    {
        // 활성 퀘스트의 복사본을 만들어서 사용
        var activeQuestsList = activeQuests.Values.ToList();

        foreach(var quest in activeQuestsList)
        {
            foreach(var condition in quest.GetConditions())
            {
                if(condition is KillQusetCondition killCondition)
                {
                    killCondition.EnemyKilled(enemytype);
                    UpdateQuestProgreses(quest.Id);
                }
            }
        }
    }

    // 수집 시 호출되는 이벤트 핸들러
    public void OnItemCollected (string itemid)
    {
        // 활성 퀘스트의 복사본을 만들어서 사용
        var activeQuestsList = activeQuests.Values.ToList();

        foreach (var quest in activeQuestsList)
        {
            foreach (var condition in quest.GetConditions())
            {
                if (condition is CollectionQusetCondition collectCondition)
                {
                    collectCondition.ItemCollected(itemid);
                    UpdateQuestProgreses(quest.Id);
                }
            }
        }
    }
}
