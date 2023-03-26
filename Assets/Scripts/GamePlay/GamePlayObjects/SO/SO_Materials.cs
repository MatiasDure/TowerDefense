using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Material",menuName = "ScriptableObjects/Materials")]
public class SO_Materials : ScriptableObject
{
    [SerializeField] public Mats[] materials;
}

