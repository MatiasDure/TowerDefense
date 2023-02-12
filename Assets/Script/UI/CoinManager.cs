using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private List<IDropMoney> moneyDroppers = new();

    private Dictionary<GameObject, CoinDropUI> gameObjCoinUi = new();

    private void Start() => UpdateSubscription(true);

    private void UpdateSubscription(bool subscribe)
    {
        if(subscribe) moneyDroppers = FindObjectsOfType<MonoBehaviour>(true).OfType<IDropMoney>().ToList();

        foreach (IDropMoney dropper in moneyDroppers)
        {
            if (subscribe) dropper.OnDropMoney += DisplayCoin;
            else dropper.OnDropMoney -= DisplayCoin;
        }
    }

    private void DisplayCoin(MoneyDroppedInfo args)
    {
        GameObject obj = ObjectPooler.Instance.GetPooledObject(ObjectPooler.PoolObjType.CoinUi);
        CoinDropUI coinUi = GetCoinScript(obj);
        coinUi.AppearCoinUiAtBody(args.ObjCalled.transform, (uint)args.Money);
    }

    private CoinDropUI GetCoinScript(GameObject obj)
    {
        if (gameObjCoinUi.ContainsKey(obj)) return gameObjCoinUi[obj];

        gameObjCoinUi.Add(obj, obj.GetComponent<CoinDropUI>());
        return gameObjCoinUi[obj];
    }

    private void OnDestroy() => UpdateSubscription(false);

}
