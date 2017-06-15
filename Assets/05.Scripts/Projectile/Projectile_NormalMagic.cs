using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_NormalMagic : Projectile {

    protected BoxCollider2D boxColl;

    protected override void Awake () {
        base.Awake();
        boxColl = GetComponent<BoxCollider2D>();
    }

    protected override void Update () {
        base.Update();
	}

    public override void Initialize(Unit parent)
    {
        base.Initialize(parent);

        boxColl.enabled = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            Monster targetMonster = collision.gameObject.GetComponent<Monster>();

            if (targetMonster.isActive)
            {
                isStop = true;
                boxColl.enabled = false;

                targetMonster.TakeDamage(parent.damage);
                myAnimator.Play("Puff");
                StartCoroutine(Puffing());
            }
        }
    }

}
