using UnityEngine;


public class MonoBehaviourSingleton<T> : MonoBehaviour where T:Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance != null) return _instance;
            var newGo = new GameObject(typeof(T).Name);
            _instance = newGo.AddComponent<T>();
            return _instance;
        }
    }
}
