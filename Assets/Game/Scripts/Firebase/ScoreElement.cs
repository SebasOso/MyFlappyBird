using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text usernameText;
    public TMP_Text pointsText;

    public void NewScoreElement(string _username, int points)
    {
        usernameText.text = _username;
        pointsText.text = points.ToString();
    }
}
