using UnityEngine;

///<summary>
///The Upgrador class is responsible for creating instances of updgraded towers
///</summary>
public class Upgrador : MonoBehaviour
{
    private TowerManager _towerUpgrade;

    ///<summary>
    ///Creates a new instance of the upgraded tower based on towerUpgrade prefab
    ///</summary>
    ///<returns>The new instance of the upgraded tower</returns>
    public TowerManager CreateUpdatedTower()
    {
        if (_towerUpgrade == null) return null;

        TowerManager updatedVersionInstance = Instantiate(_towerUpgrade);
        updatedVersionInstance.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        return updatedVersionInstance;
    }

    ///<summary>
    ///Sets the upgraded version of the tower
    ///</summary>
    ///<param name="upgradedVersion">The upgraded version of the tower</param>
    public void SetUpgradedVersion(TowerManager upgradedVersion) => _towerUpgrade = upgradedVersion;
}
