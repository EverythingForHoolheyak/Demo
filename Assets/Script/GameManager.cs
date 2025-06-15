using UnityEngine;
using Utils;

namespace managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] public int SpawnObjectCountInASecond = 1;
        [SerializeField] private string pooledObjectId = "Cube";
        private float spawnTimer = 0f;

        private void Update()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1f)
            {
                spawnTimer -= 1f;
                for (var i = 0; i < SpawnObjectCountInASecond; i++)
                {
                    SpawnObject.Instance.Spawn(pooledObjectId);
                }
            }
        }
    }
}