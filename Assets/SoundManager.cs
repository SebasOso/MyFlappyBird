using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {  get; private set; }

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip loseClip;
    [SerializeField] private AudioClip winClip;

    [SerializeField] private AudioSource jumpSource;
    [SerializeField] private AudioSource loseSource;
    [SerializeField] private AudioSource winSource;
    private void Awake()
    {
        Instance = this;
        LoseManager.OnDie += PlayLose;
        PointsManager.OnPoint += PlayWin;
    }
    private void OnDestroy()
    {
        LoseManager.OnDie -= PlayLose;
        PointsManager.OnPoint -= PlayWin;
    }
    public void PlayJump()
    {
        jumpSource.clip = jumpClip;
        jumpSource.Play();
    }
    public void PlayWin()
    {
        winSource.clip = winClip;
        winSource.Play();
    }
    public void PlayLose()
    {
        loseSource.clip = loseClip;
        loseSource.Play();
    }
}
