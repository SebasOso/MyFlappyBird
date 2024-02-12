using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    //Singleton
    public static PointsManager Instance { get; private set; }

    [Header("Points Settings")]
    [SerializeField] private int pointsToAdd;
    [SerializeField] private int totalPoints;

    public static event Action OnPoint;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(LoseManager.Instance.isDead == true) { return; }
        if(collision.CompareTag("win"))
        {
            AddPoint();
        }
    }
    private void AddPoint()
    {
        totalPoints += pointsToAdd;
        OnPoint?.Invoke();
    }
    public int GetPoints()
    {
        return totalPoints;
    }
}
