using UnityEngine;

namespace Utils
{
    public class Singleton<T> where T : class
    {
        private static T instance;
        public static T Instance => instance;

        public static void SetInstance(T newInstance)
        {
            if (instance == null)
            {
                instance = newInstance;
            }
            else
            {
                Debug.LogWarning($"Singleton<{typeof(T).Name}> already has an instance.");
            }
        }
    }
}
