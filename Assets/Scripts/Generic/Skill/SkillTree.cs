using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillNode
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public object Skill { get; private set; }
    public List<string> RequiredSkillds { get; private set; }
    public bool isUnlocked { get; set; }
    public Vector2 Position { get; private set; }
    public string SkillSeries { get; private set; }
    public int SkillLevel { get; private set; }
    public bool IsMaxLevel { get; set; }

    public SkillNode(string id, string name, object skill, Vector2 positoin, string skillSeries, int skillLevel, List<string> requiredSkillds = null)
    {
        Id = id;
        Name = name;
        Skill = skill;
        Position = positoin;
        SkillSeries = skillSeries;
        RequiredSkillds = requiredSkillds ?? new List<string>();
        isUnlocked = false;
    }
}

public class SkillTree //특성 트리 클래스
{
    public List<SkillNode> Nodes { get; private set; } = new List<SkillNode>();
    private Dictionary<string, SkillNode> nodeDictionary;

    public SkillTree()
    {
        Nodes = new List<SkillNode>();
        nodeDictionary = new Dictionary<string, SkillNode>();
    }

    public void AddNode(SkillNode node)
    {
        Nodes.Add(node);
        nodeDictionary[node.Id] = node;
    }

    public bool UnlockSkill(string skilld)
    {
        if (nodeDictionary.TryGetValue(skilld, out SkillNode node))
        {
            if (node.isUnlocked) return false;

            foreach (var requiredSkilld in node.RequiredSkillds)
            {
                if (!nodeDictionary[requiredSkilld].isUnlocked)
                {
                    return false;
                }
            }
            node.isUnlocked = true;
            return true;
        }
        return false;
    }
    public bool LockSkill(string skilld)
    {
        if (nodeDictionary.TryGetValue(skilld,out SkillNode node))
        {
            if(!node.isUnlocked) return false ;

            foreach (var otherNode in Nodes)
            {
                if(otherNode.isUnlocked && otherNode.RequiredSkillds.Contains(skilld))
                {
                    return false;
                }
            }
            node.isUnlocked = false;
            return true;
        }
        return false;
    }

    public bool IsSkillUnlock(string skilld)
    {
        return nodeDictionary.TryGetValue(skilld, out SkillNode node) && node.isUnlocked;
    }

    public SkillNode GetNode(string skilld)
    {
        nodeDictionary.TryGetValue(skilld, out SkillNode node);
        return node;
    }

    public List<SkillNode> GetAllNodes()
    {
        return new List<SkillNode>(Nodes);
    }
}
