using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] GameObject coin;
    private void OnEnable()
    {
        coin.SetActive(true);
        StartCoroutine(Desactivate());
    }
    private IEnumerator Desactivate()
    {
        yield return new WaitForSeconds(5f);
        ObstacleSpawner.Instance.SpawnObstacle();
        gameObject.SetActive(false);
    }

}
