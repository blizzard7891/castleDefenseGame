using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleUnit : Unit {
    public string unitType;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}

    protected override void Shoot()
    {
        base.Shoot();

        if (unitType.Equals("Bow"))
        {
            SoundManager.Instance.PlaySFX("bow_s");
        }
    }
}
