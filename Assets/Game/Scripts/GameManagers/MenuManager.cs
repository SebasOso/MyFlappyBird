using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Transform scoreBoardContent;
    [SerializeField] GameObject scoreElement;
    public void StartGame()
    {
        int levelToLoad = Random.Range(2, 4); 
        SceneManager.LoadScene(levelToLoad);
    }
    public void UpdateScoreBoard()
    {
        scoreBoardContent.transform.position = new Vector3(scoreBoardContent.transform.position.x, -1584.931f, 0f);
        FirebaseManager.Instance.LoadScoreBoard(scoreBoardContent, scoreElement);
    }
}
