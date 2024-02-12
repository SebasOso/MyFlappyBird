using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 5f)]
    [SerializeField] private float speed;

    public bool canFly = true;

    //Variables
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (LoseManager.Instance.isDead == true) { return; }
        if (rb.velocity.y <= 0f )
        {
            //animator.SetBool("isFlying", false);
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                AddVelocity();
                /*if (canFly)
                {
                    Fly();
                }*/
            }
        }

        #if UNITY_EDITOR
            if(Input.GetMouseButtonDown(0))
            {
                AddVelocity();
                /*if (canFly)
                {
                    Fly();
                }*/
            }
#endif
    }
    public void Fly()
    {
        animator.SetBool("isFlying", true);
    }
    public void AddVelocity()
    {
        SoundManager.Instance.PlayJump();
        rb.velocity = Vector3.up * speed;
    }
}
