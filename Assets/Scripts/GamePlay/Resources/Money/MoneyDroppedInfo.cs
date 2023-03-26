using UnityEngine;

public struct MoneyDroppedInfo
{
    public float Money { get; private set; }
    public GameObject ObjCalled { get; private set; }
    public MoneyDroppedInfo(GameObject objCalled, float money)
    {
        this.Money = money;
        this.ObjCalled = objCalled;
    }
}
