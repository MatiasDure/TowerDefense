using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helps in grouping materials that might be in used together
/// </summary>
[CreateAssetMenu(fileName = "SO_Material",menuName = "ScriptableObjects/Materials")]
public class SO_Materials : ScriptableObject
{
    [SerializeField] public Mats[] materials;
}

