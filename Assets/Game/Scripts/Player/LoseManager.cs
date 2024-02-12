using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseManager : MonoBehaviour
{
    public static event Action OnDie;

    [SerializeField] private AudioSource music;
    //Variables
    private Animator animator;
    private Rigidbody2D rb;

    [HideInInspector] public bool isDead = false;

    public static LoseManager Instance { get; private set; }
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

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) { return; }
        Debug.Log("MORISTE");
        music.Stop();
        OnDie?.Invoke();
        StartCoroutine(Die());
    }
    private IEnumerator Die()
    {
        FirebaseManager.Instance.UpdateNewPoints(PointsManager.Instance.GetPoints());   
        DesactivateAll();
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(1);
    }
    private void DesactivateAll()
    {
        isDead = true;
        animator.SetTrigger("die");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.AddForce(new Vector2(0, -3f));
    }
}
