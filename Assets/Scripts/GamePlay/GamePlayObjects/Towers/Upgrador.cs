using UnityEngine;

public class Upgrador : MonoBehaviour
{
    private TowerManager towerUpgrade;

    public TowerManager CreateUpdatedTower()
    {
        if (towerUpgrade == null) return null;

        TowerManager updatedVersionInstance = Instantiate(towerUpgrade);
        updatedVersionInstance.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);

        return updatedVersionInstance;
    }

    public void SetUpgradedVersion(TowerManager upgradedVersion) => towerUpgrade = upgradedVersion; 

}
