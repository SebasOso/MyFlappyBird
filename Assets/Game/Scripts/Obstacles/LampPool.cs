using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPool : MonoBehaviour
{
    //Singleton
    public static LampPool Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] private GameObject lampPrefab;
    [SerializeField] private int poolSize = 10;

    [Header("Debugging")]
    [SerializeField] private List<GameObject> lampList;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        AddLampToPool(poolSize);
    }
    private void AddLampToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject lamp = Instantiate(lampPrefab);
            lamp.SetActive(false);
            lampList.Add(lamp);
            lamp.transform.parent = transform;
        }
    }
    public GameObject RequestLamp()
    {
        Debug.Log("Called");
        for (int i = 0; i < lampList.Count; i++)
        {
            if (!lampList[i].activeSelf)
            {
                Debug.Log("Activated");
                lampList[i].SetActive(true);
                return lampList[i];
            }
        }
        return null;
    }
}
