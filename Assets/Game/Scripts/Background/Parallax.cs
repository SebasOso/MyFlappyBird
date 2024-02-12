using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght;
    private float startPos;

    [Header("Settings")]
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private bool isMainMenu;

    private void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        if(!isMainMenu)
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float dist = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

            if (temp > startPos + lenght)
            {
                Debug.Log("temp derecho: " + temp);
                startPos += lenght;
            }
            else if (temp < startPos - lenght)
            {
                Debug.Log("temp izquierdo: " + temp);
                startPos -= lenght;
            }
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * 1f);

            if (transform.position.x > 10)
            {
                transform.position = new Vector3(startPos - 26, transform.position.y, transform.position.z);
            }
        }
    }
}
