using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Arrow : Projectile {
    [SerializeField]
    private float angle;

    private Rigidbody2D rbody;
    protected BoxCollider2D boxColl;


    private bool isClose = false;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        rbody = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isClose)
        {
            base.MoveToTarget();
        }
        Rotate();
    }

    public override void Initialize(Unit parent)
    {
        base.Initialize(parent);

        isClose = false;
        boxColl.enabled = true;
        rbody.gravityScale = 1;

        destPos = target.transform.position;

        MoveToTarget();
    }

    protected override void MoveToTarget()
    {
        Vector2 dir = destPos - transform.position;
        var h = dir.y;
        dir.y = 0;
        var dist = dir.magnitude;
        var a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += h / Mathf.Tan(a);


        if(dist <= 20)
        {
            destPos = target.transform.position + new Vector3(0, target.Y / 2, 0);
            rbody.gravityScale = 0;
            isClose = true;
        }
        else
        {
            var vel = Mathf.Sqrt(dist * Physics2D.gravity.magnitude / Mathf.Sin(2 * a));

            rbody.velocity = vel * dir.normalized;
        }
    }

    public void Rotate()
    {
        var dir = rbody.velocity;

        if (dir != Vector2.zero)
        {
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            Monster targetMonster = collision.gameObject.GetComponent<Monster>();

            if (targetMonster.isActive)
            {
                isStop = true;
                rbody.velocity = Vector2.zero;
                rbody.gravityScale = 0;
                boxColl.enabled = false;

                targetMonster.TakeDamage(parent.damage);
                myAnimator.Play("Puff");
                StartCoroutine(Puffing());
            }
        }else if (collision.tag == "Ground")
        {
            rbody.velocity = Vector2.zero;
            rbody.gravityScale = 0;
            boxColl.enabled = false;

            //myAnimator.Play("Puff");
            StartCoroutine(Puffing());
        }
    }
}
