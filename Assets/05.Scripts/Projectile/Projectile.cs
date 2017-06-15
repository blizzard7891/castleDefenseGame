using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Projectile : MonoBehaviour {
    protected Monster target;
    protected Unit parent;
    protected tk2dSpriteAnimator myAnimator;
    protected bool isStop = false;
    protected Vector3 destPos;


    protected virtual void Awake()
    {
        myAnimator = GetComponent<tk2dSpriteAnimator>();
    }


    // Update is called once per frame
    protected virtual void Update () {
        MoveToTarget();
    }

    public virtual void Initialize(Unit parent)
    {
        isStop = false;

        myAnimator.Play("Idle");
        this.target = parent.target;
        this.parent = parent;

        destPos = target.transform.position + new Vector3(0, target.Y / 2, 0);
    }

    protected virtual void MoveToTarget()
    {
        if(!isStop)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * parent.projectileSpeed);
            Vector2 dir = destPos - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
          
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (transform.position == destPos)   
        {
            myAnimator.Play("Puff");
            StartCoroutine(Puffing());
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            Monster tagetMonster = collision.gameObject.GetComponent<Monster>();

            if (tagetMonster.isActive)
            {
                isStop = true;

                tagetMonster.TakeDamage(parent.damage);
                myAnimator.Play("Puff");
                StartCoroutine(Puffing());
            }else
            {
                isStop = true;
                myAnimator.Play("Puff");
                StartCoroutine(Puffing());
            }
        }
    }

    protected virtual IEnumerator Puffing()
    {
        float progress = 0;

        while (progress <= 0.8)
        {
            progress += Time.deltaTime * 1;
            yield return null;
        }

        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
}
