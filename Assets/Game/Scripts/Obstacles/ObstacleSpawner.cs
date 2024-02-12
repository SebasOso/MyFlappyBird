using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }
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
        StartCoroutine(Spawn());
    }
    public void SpawnObstacle()
    {
        GameObject lamp = LampPool.Instance.RequestLamp();
        if (lamp == null)
        {
            Debug.Log("No lamp found");
        }
        lamp.transform.position = new Vector3(Camera.main.transform.position.x + 5, Camera.main.transform.position.y + (Random.Range(-1.7f, 1.7f)), 0);
    }
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2);
        SpawnObstacle();
    }
}
