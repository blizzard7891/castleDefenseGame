using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_SplashMagic : Projectile {

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override IEnumerator Puffing()
    {
        float progress = 0;

        while (progress <= 1.2)
        {
            progress += Time.deltaTime * 1;
            yield return null;
        }

        GameManager.Instance.Pool.ReleaseObject(gameObject);
    }
}
