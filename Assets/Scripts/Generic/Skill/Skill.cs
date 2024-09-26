using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

//��ų Ÿ�� �������̽�
public interface ISkillTarget
{
    void ApplyEffect(ISkillEffect effect);
}

//��ų ȿ�� �������̽�
public interface ISkillEffect
{
    void Apply (ISkillTarget target);
}

// ��ü���� ȿ�� Ŭ����
public class DamageEffect : ISkillEffect
{
    public int Damge { get; private set; }

    public DamageEffect(int damage)
    {
        Damge = damage;
    }

    public void Apply(ISkillTarget target)
    {
        if (target is PlayerTarget playerTarget)
        {
            playerTarget.Health -= Damge;
            Debug.Log($"Player look {Damge} damage. Remaining health : {playerTarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health -= Damge;
            Debug.Log($"Enemy look {Damge} damage. Remaining health : {enemyTarget.Health}");
        }
    }
}

public class HealEffect : ISkillEffect
{
    public int HealAmount { get; private set; }

    public HealEffect(int damage)
    {
        HealAmount = damage;
    }

    public void Apply(ISkillTarget target)
    {
        if (target is PlayerTarget playerTarget)
        {
            playerTarget.Health += HealAmount;
            Debug.Log($"Player healed for {HealAmount}. Remaining health : {playerTarget.Health}");
        }
        else if (target is EnemyTarget enemyTarget)
        {
            enemyTarget.Health += HealAmount;
            Debug.Log($"Enemy healed for {HealAmount}. Remaining health : {enemyTarget.Health}");
        }
    }
}

//���׸� ��ų Ŭ����
public class Skill<TTarget, TEffect>
    where TTarget : ISkillTarget
    where TEffect : ISkillEffect
{
    public string Name { get; private set; }
    public TEffect Effect { get; private set; }

    public Skill(string name, TEffect effect)
    {
        Name = name;
        Effect = effect;
    }

    public void Use(TTarget target)
    {
        Debug.Log($"Using skill : {Name}");
        target.ApplyEffect(Effect);
    }
}