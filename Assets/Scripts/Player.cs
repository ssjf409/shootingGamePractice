using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchedTop;
    public bool isTouchedBottom;
    public bool isTouchedRight;
    public bool isTouchedLeft;
    
    public float speed;
    public float power;
    public float maxShotDelay;
    public float curShotDelay;
    
    private Animator _animator;
    [SerializeField] private GameObject _bulletObjectA;
    [SerializeField] private GameObject _bulletObjectB;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
    }


    private void Move()
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
    
    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        switch (power)
        {
            case 1:
                GameObject bullet = Instantiate(_bulletObjectA, transform.position, transform.rotation);
                Rigidbody2D rigidbody2D = bullet.GetComponent<Rigidbody2D>();
                rigidbody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletLeft = Instantiate(_bulletObjectA, transform.position + Vector3.left * 0.1f, transform.rotation);
                GameObject bulletRight = Instantiate(_bulletObjectA, transform.position + Vector3.right * 0.1f, transform.rotation);
                Rigidbody2D rigidbody2DLeft = bulletLeft.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidbody2DRight = bulletRight.GetComponent<Rigidbody2D>();
                rigidbody2DLeft.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidbody2DRight.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletLL = Instantiate(_bulletObjectA, transform.position + Vector3.left * 0.35f, transform.rotation);
                GameObject bulletRR = Instantiate(_bulletObjectA, transform.position + Vector3.right * 0.35f, transform.rotation);
                GameObject bulletCC = Instantiate(_bulletObjectB, transform.position, transform.rotation);
                Rigidbody2D rigidbody2DLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidbody2DRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidbody2DCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidbody2DLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidbody2DRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidbody2DCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotDelay = 0;
    }
    
    private void Reload()
    {
        curShotDelay += Time.deltaTime;
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
