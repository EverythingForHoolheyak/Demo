using System;
using System.Collections;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private string pooledObjectId;

    private float currentX = 0;

    private void Start()
    {
        // Tự động tìm ObjectPool nếu chưa gán
        if (objectPool == null)
        {
            objectPool = FindObjectOfType<ObjectPool>();
            if (objectPool == null)
            {
                Debug.LogError("Không tìm thấy ObjectPool trong scene!");
            }
        }
    }

    /// <summary>
    /// Spawn một object từ pool và đặt vị trí.
    /// </summary>
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

    /// <summary>
    /// Trả object về lại pool sau 1 giây.
    /// </summary>
    private IEnumerator ReturnObject(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        if (objectPool != null)
        {
            objectPool.ReturnObject(pooledObjectId, go);
        }
        currentX = 0;
    }

    /// <summary>
    /// Nút test spawn trên màn hình.
    /// </summary>
    private void OnGUI()
    {
        if (GUILayout.Button("Spawn Object"))
        {
            Spawn(pooledObjectId);
        }
    }
}
