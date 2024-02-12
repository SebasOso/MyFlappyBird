using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject registerScreen;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject loginScreen;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void MainScreen()
    {
        registerScreen.SetActive(false);
        mainScreen.SetActive(true);
        loginScreen.SetActive(false);
    }
    public void LoginScreen()
    {
        registerScreen.SetActive(false);
        mainScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
    public void RegisterScreen()
    {
        registerScreen.SetActive(true);
        mainScreen.SetActive(false);
    }
}
