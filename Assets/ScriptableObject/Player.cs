using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/Player")]
public class Player : ScriptableObject
{
    public int wallRunVelocity;
    public int velocity;
    public int hp;
    public int dashForce;
    public int jumpForce;
    public float nitroso;
    public bool invertY;
    public float recoil;
}

