using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptables/EnemyScriptable")]
public class EnemyScriptable : ScriptableObject
{
    public float vida;
    public float rangeVision;
    public float rangeRandom;
    public float angularVelocity;
    public int Damage;

}
