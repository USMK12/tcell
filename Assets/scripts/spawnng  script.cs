using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabsOnCylinderSurface : MonoBehaviour
{
    [SerializeField] private GameObject prefab1; 
    [SerializeField] private GameObject prefab2; 
    [SerializeField] private GameObject prefab3;
    [SerializeField] private int spawnCount1Initial = 20; 
    [SerializeField] private int spawnCount2 = 5;   
    [SerializeField] private int spawnCount3 = 3;  
    [SerializeField] private float cylinderRadius = 1.0f;
    [SerializeField] private float cylinderHeight = 2.0f; 
    [SerializeField] private LayerMask collisionLayerMask; 
    [SerializeField] private float avoidanceForce = 1.0f; 

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Track spawned objects

    private void Start()
    {
       
        for (int i = 0; i < spawnCount1Initial; i++)
        {
            SpawnPrefab(prefab1);
        }

        
        for (int i = 0; i < spawnCount2; i++)
        {
            SpawnPrefab(prefab2);
        }

        for (int i = 0; i < spawnCount3; i++)
        {
            SpawnPrefab(prefab3);
        }

       
        StartCoroutine(SpawnExtraPrefab1());
    }

    private void Update()
    {
        
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            for (int j = 0; j < spawnedObjects.Count; j++)
            {
                if (i != j)
                {
                    AvoidCollision(spawnedObjects[i], spawnedObjects[j]);
                }
            }
        }
    }

    private void SpawnPrefab(GameObject prefab)
    {
        Vector3 spawnPosition = Vector3.zero; 
        bool validPositionFound = false;
        int maxAttempts = 100;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float randomHeight = Random.Range(-cylinderHeight * 0.5f, cylinderHeight * 0.5f);
            float randomAngle = Random.Range(0.0f, 360.0f);

            spawnPosition = transform.position + new Vector3(Mathf.Cos(randomAngle) * cylinderRadius, randomHeight, Mathf.Sin(randomAngle) * cylinderRadius);

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, prefab.GetComponent<Collider>().bounds.size.x * 0.5f, collisionLayerMask);
            validPositionFound = colliders.Length == 0;

            if (validPositionFound)
            {
                break;
            }
        }

        if (validPositionFound)
        {
            GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity); // Use identity for flat placement on surface
            spawnedObjects.Add(spawnedObject);
        }
        else
        {
            Debug.LogWarning($"Failed to find non-colliding position for {prefab.name} on cylinder surface after {maxAttempts} attempts.");
        }
    }

    IEnumerator SpawnExtraPrefab1()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f); 
            SpawnPrefab(prefab1);
        }
    }

    private void AvoidCollision(GameObject obj1, GameObject obj2)
    {
        if (obj1 == null || obj2 == null)
        {
            return;
        }

        Vector3 direction = obj1.transform.position - obj2.transform.position;
        float distance = direction.magnitude;

        if (obj1 == null || obj2 == null)
        {
            return;
        }

        if (distance < obj1.GetComponent<Collider>().bounds.size.x * 0.5f + obj2.GetComponent<Collider>().bounds.size.x * 0.5f)
        {
            Vector3 avoidanceForceVector = direction.normalized * avoidanceForce;
            obj1.transform.position += avoidanceForceVector * Time.deltaTime;
        }
    }

}
