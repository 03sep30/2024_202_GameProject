using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ISkillTarget
{
    public int Health { get; set; } = 150;

    public void ApplyEffect(ISkillEffect effect)
    {
        effect.Apply(this);
    }
}
