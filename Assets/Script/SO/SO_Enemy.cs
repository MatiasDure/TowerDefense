using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "ScriptableObjects/Enemy")]
public class SO_Enemy : ScriptableObject
{
    [SerializeField] string _id = "";
    [SerializeField] int _damage = 0;

    public string ID => _id;
    public int Damage => _damage;
}
