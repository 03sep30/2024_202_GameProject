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


    // �⺻ ����Ʈ���� �����ϰ� ����ϴ� �޼���
    private void InitializeQuests()
    {
        // �� ��� ����Ʈ ���� ����
        var ratHuntQuest = new Quest("Q001", "Rat Problem", "Clear the basement of rate", QuestType.Kill, 1);
        ratHuntQuest.Addcondition(new KillQusetCondition("Rat", 5));
        ratHuntQuest.AddReward(new ExperienceReward(100));
        ratHuntQuest.AddReward(new ItemReward("Gold", 50));

        // ���� ���� ����Ʈ
        var herbQuest = new Quest("Q002", "Herb Collection", "Collect herbs for the healer", QuestType.Collection, 1);
        herbQuest.Addcondition(new CollectionQusetCondition("Herb", 3));
        herbQuest.AddReward(new ExperienceReward(50));

        // ����Ʈ �Ŵ����� ����Ʈ �߰�
        allQuests.Add(ratHuntQuest.Id, ratHuntQuest);
        allQuests.Add(herbQuest.Id, herbQuest);

        // �׽�Ʈ ���ؼ� �ٷ� ���� (StartQuest �Լ�)
        StartQuest("Q001");
        StartQuest("Q002");

    }

    public bool CanStartQuest(string questId)       // Ư�� ����Ʈ�� ������ �� �ִ��� �˻��ϴ� �޼���
    {
        if(!allQuests.TryGetValue(questId, out Quest quest)) return false;
        if(activeQuests.ContainsKey(questId)) return true;
        if(completedQuests.ContainsKey(questId)) return true;

        // ���� ����Ʈ �Ϸ� ���� Ȯ��
        foreach (var perrequistiteId in quest.GetType().GetField("prerequisiteQuestIds")?.GetValue(quest) as List<string> ?? new List<string>())
        {
            if(!completedQuests.ContainsKey(perrequistiteId)) return false;
        }

        //Type questType = quest.GetType();                                                         // Quest ��ü�� Ÿ���� �����´�.           
        //FieldInfo perrequisiteIdsField = GetField("prerequisiteQuestIds");                        // Quest Type ���� �ʵ带 �˻�
        //object perrequisiteIdsValue = perrequisiteIdsField.GetValue(quest);                       // �ʵ� ���� �����´�.
        //List<string> prerequisiteQuestIds = perrequisiteIdsValue as List<string>();               // List�� ��ȯ�ϰ�
        //prerequisiteQuestIds = prerequisiteQuestIds ?? new List<string>();                        // Null ���� ��쿡�� �� List�� ����Ѵ�.
        // ?? Null ���� ������ -> ���� ���� Null���� Ȯ���Ͽ� Null�� ��� ���������� ��ȯ

        return true;

    }

    // ����Ʈ�� �����ϴ� �޼���
     public void StartQuest(string questId)
     {
        if(!CanStartQuest(questId)) return;
       
         var quest = allQuests[questId];
         quest.Start();
         activeQuests.Add(questId, quest);
         OnQuestStarted?.Invoke(quest);
       
     }

    // ����Ʈ ���� ��Ȳ�� ������Ʈ �ϴ� �޼���
    public void UpdateQuestProgreses(string questId)
    {
        if(!activeQuests.TryGetValue(questId,out Quest quest)) return;
        
        if(quest.CheckCompletion())
        {
            CompleteQuest(questId);
        }
    }

    // ����Ʈ�� �Ϸ� ó�� �ϴ� �޼���
    private void CompleteQuest(string questId)
    {
        if (!activeQuests.TryGetValue(questId, out Quest quest)) return;

        // �÷��̾� ã�� �����ص� ����Ʈ�� �Ϸ�
        var player = GameObject.FindGameObjectWithTag("Player");
        quest.Complete(player);

        activeQuests.Remove(questId);
        completedQuests.Add(questId, quest);
        OnQuestCompleted?.Invoke(quest);

        Debug.Log($"Quest completed : { quest.Title}");
    }

    // ���� ������ ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetAvailableQuests()
    {
        return allQuests.Values.Where(q => CanStartQuest(q.Id)).ToList();
    }

    // ���� ���� ���� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetActiveQuest()
    {
        return activeQuests.Values.ToList();
    }

    // �Ϸ�� ����Ʈ ����� ��ȯ�ϴ� �޼���
    public List<Quest> GetCompletedQuest()
    {
        return completedQuests.Values.ToList();
    }


    // �� óġ �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    public void OnEnemyKilled(string enemytype)
    {
        // Ȱ�� ����Ʈ�� ���纻�� ���� ���
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

    // ���� �� ȣ��Ǵ� �̺�Ʈ �ڵ鷯
    public void OnItemCollected (string itemid)
    {
        // Ȱ�� ����Ʈ�� ���纻�� ���� ���
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
