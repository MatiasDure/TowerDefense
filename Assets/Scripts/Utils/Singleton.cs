using UnityEngine;

/// <summary>
/// A generic singleton class for Unity that ensures only one instance of a
/// MonoBehaviour-derived class exists.
/// </summary>
/// <typeparam name="T">
/// The MonoBehaviour-derived class that should be made into a singleton.
/// </typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    /// The singleton instance of the MonoBehaviour-derived class.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null) Debug.LogError($"Unable to find instance of {typeof(T)}");
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null) _instance = this as T;
        else if (_instance != this)
        {
            Debug.Log(this as T + " is being destroyed!");
            Destroy(this.gameObject);
            return;
        }
    }
}