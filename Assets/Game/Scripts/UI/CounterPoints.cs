using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterPoints : MonoBehaviour
{
    private void Awake()
    {
        PointsManager.OnPoint += PointsUpdate;
    }
    private void OnDestroy()
    {
        PointsManager.OnPoint -= PointsUpdate;
    }
    private void PointsUpdate()
    {
        GetComponent<TextMeshProUGUI>().text = PointsManager.Instance.GetPoints().ToString();
    }
}
