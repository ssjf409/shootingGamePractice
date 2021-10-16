using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public bool isTouchedTop;
    public bool isTouchedBottom;
    public bool isTouchedRight;
    public bool isTouchedLeft;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchedRight && h == 1) || (isTouchedLeft && h == -1))
        {
            h = 0;
        }
        float v = Input.GetAxisRaw("Vertical");
        
        if ((isTouchedTop && v == 1) || (isTouchedBottom && v == -1))
        {
            v = 0;
        }
        
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;


        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            _animator.SetInteger("Input", (int)h);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchedTop = true;
                    break;
                case "Bottom":
                    isTouchedBottom = true;
                    break;
                case "Right":
                    isTouchedRight = true;
                    break;
                case "Left":
                    isTouchedLeft = true;
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchedTop = false;
                    break;
                case "Bottom":
                    isTouchedBottom = false;
                    break;
                case "Right":
                    isTouchedRight = false;
                    break;
                case "Left":
                    isTouchedLeft = false;
                    break;
            }
        }
    }
}
