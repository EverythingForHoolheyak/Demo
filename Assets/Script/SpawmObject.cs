using System.Collections;
using UnityEngine;
using Utils;

public class SpawnObject : MonoSingleton<SpawnObject>
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private string pooledObjectId;

    private float currentX = 0;

    private void Start()
    {
        if (objectPool == null)
        {
            objectPool = FindObjectOfType<ObjectPool>();
            if (objectPool == null)
            {
                Debug.LogError("Không tìm thấy ObjectPool trong scene!");
            }
        }
    }

    public void Spawn(string pooledObjectId)
    {
        if (objectPool == null)
        {
            Debug.LogError("objectPool chưa được gán!");
            return;
        }

        var go = objectPool.GetObject(pooledObjectId);
        if (go != null)
        {
            go.transform.position = new Vector3(currentX, 0, 0);
            go.transform.rotation = Quaternion.identity;
            currentX++;
            StartCoroutine(ReturnObject(go));
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy object với ID: {pooledObjectId}");
        }
    }

    private IEnumerator ReturnObject(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        if (objectPool != null)
        {
            objectPool.ReturnObject(pooledObjectId, go);
        }
        currentX = 0;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Spawn Object"))
        {
            Spawn(pooledObjectId);
        }
    }
}
