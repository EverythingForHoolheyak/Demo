using UnityEngine;

namespace Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning($"An instance of {typeof(T).Name} already exists. Destroying duplicate.");
                Destroy(gameObject);
            }
        }
    }
}
