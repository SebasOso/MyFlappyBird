using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Settings")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float velocity;

    private Camera m_Camera;
    private float screenWidth;
    private void Awake()
    {
        LoseManager.OnDie += StopCamera;
    }
    private void OnDestroy()
    {
        LoseManager.OnDie -= StopCamera;
    }
    private void Start()
    {
        m_Camera = Camera.main;
        screenWidth = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - m_Camera.ScreenToWorldPoint(Vector3.zero).x;
    }

    private void Update()
    {
        float newXPosition = m_Camera.transform.position.x + velocity * Time.deltaTime;


        m_Camera.transform.position = new Vector3(newXPosition, 0, -10);
    }
    private void StopCamera()
    {
        velocity = 0;
    }
}
