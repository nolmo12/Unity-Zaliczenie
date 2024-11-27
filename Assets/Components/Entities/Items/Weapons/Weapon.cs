using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class Weapon : Item
{
    public float AttackSpeed;
    public float Damage;
    public float Range;
}
