using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///<summary>
///Manages the coin drops ui in the game by subscribing to the <c>IDropMoney</c> events.
///</summary>
public class CoinManager : MonoBehaviour
{
    private List<IDropMoney> _moneyDroppers = new();

    private Dictionary<GameObject, CoinDropUI> _gameObjCoinUi = new();

    private void Start() => UpdateSubscription(true);

    ///<summary>
    ///Update the subscription to the <c>IDropMoney</c> events.
    ///</summary>
    ///<param name="subscribe">True to subscribe, False to unsubscribe.</param>
    private void UpdateSubscription(bool subscribe)
    {
        if (subscribe) _moneyDroppers = FindObjectsOfType<MonoBehaviour>(true).OfType<IDropMoney>().ToList();

        foreach (IDropMoney dropper in _moneyDroppers)
        {
            if (subscribe) dropper.OnDropMoney += DisplayCoin;
            else dropper.OnDropMoney -= DisplayCoin;
        }
    }

    ///<summary>
    ///Displays the coin ui when coins are dopped dropped.
    ///</summary>
    ///<param name="args"> Information about the coins dropped. </param>
    private void DisplayCoin(MoneyDroppedInfo args)
    {
        GameObject obj = ObjectPooler.Instance.GetPooledObject(ObjectPooler.PoolObjType.CoinUi);
        CoinDropUI coinUi = GetCoinScript(obj);

        if (coinUi == null) return;

        coinUi.AppearCoinUiAtBody(args.Owner.transform, (uint)args.Money);
    }

    ///<summary>
    ///Get the <c>CoinDropUI</c> script from an object passed as a parameter.
    ///</summary>
    ///<param name="obj"> Object to retrieve the script from. </param>
    ///<returns> The <c>CoinDropUI</c> script attached to the object passed. </returns>
    private CoinDropUI GetCoinScript(GameObject obj)
    {
        if (_gameObjCoinUi.ContainsKey(obj)) return _gameObjCoinUi[obj];

        if(obj.TryGetComponent<CoinDropUI>(out var coinDropScript))
        {
            _gameObjCoinUi.Add(obj, coinDropScript);
            return _gameObjCoinUi[obj];
        }

        return null;
    }

    private void OnDestroy() => UpdateSubscription(false);
}
