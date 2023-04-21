using UnityEngine;

/// <summary>
/// Facilitates modifying an enemy's stats
/// </summary>
[CreateAssetMenu(fileName = "SO_Enemy", menuName = "ScriptableObjects/Enemy")]
public class SO_Enemy : ScriptableObject
{
    [SerializeField] string _type = "";
    [SerializeField] int _damage = 0;

    /// <summary>
    /// Gets the type assigned of this enemy instance
    /// </summary>
    public string Type => _type;

    /// <summary>
    /// Gets the damage this enemy instance inflicts
    /// </summary>
    public int Damage => _damage;
}
