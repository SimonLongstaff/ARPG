using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjects : MonoBehaviour
{

    [SerializeField] GameObject persistentObjectPrefab;

    static bool hasSpawned;

    private void Awake()
    {
        if (hasSpawned)
        {
            return;
        }

        SpawnPersistentObjects();

        hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persitentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persitentObject);
    }
}
