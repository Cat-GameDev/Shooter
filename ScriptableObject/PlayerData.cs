using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] float hp;
    [SerializeField] float moveSpeed;
    [SerializeField] float rangeAttack;

    public float Hp { get => hp; }
    public float MoveSpeed { get => moveSpeed;}
    public float RangeAttack { get => rangeAttack;}
}
