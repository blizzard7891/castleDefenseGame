using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Unit : MonoBehaviour {

    public int damage;
    public float attackCoolTime;
    public float projectileSpeed;
    public int unitPrice;

    [SerializeField]
    protected string projectileType;
    [SerializeField]
    protected AttackRange myAttackRange;
    [SerializeField]
    public UnitUpgrade upgrade;
    [SerializeField]
    private int firstUpgradePrice;
    [SerializeField]
    private int addUpgradePrice;

    protected tk2dSpriteAnimator myAnimator;
    public Monster target;
    protected bool canAttack = true;

    public List<Monster> myTargets;

    private float attackTimer;
    private int upgradeLevel;
    private Transform projectileParent;

    protected virtual void Awake()
    {
        myAttackRange = FindObjectOfType<AttackRange>().GetComponent<AttackRange>();
        myAnimator = GetComponent<tk2dSpriteAnimator>();
        projectileParent = GameObject.Find("ProjectilePool").GetComponent<Transform>();
        upgradeLevel = 1;
        upgrade = new UnitUpgrade(firstUpgradePrice, 1);
    }

    // Use this for initialization
    protected virtual void Start () {
        myTargets = myAttackRange.targets;

        myAnimator.Play("Idle");
        myAnimator.GetClipByName("Attack").fps = myAnimator.CurrentClip.frames.Length / attackCoolTime;
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        Attack();
    }

   public void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCoolTime)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        if (target == null && myTargets.Count > 0)
        {
            if(myTargets.Count > 4)
            {
                target = myTargets[Random.Range(0, 4)];
            }
            else
            {
                target = myTargets[Random.Range(0, myTargets.Count)];
            }

        }

        if (target != null && target.isActive)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
        else if (myTargets.Count > 0)
        {
            if (myTargets.Count > 4)
            {
                target = myTargets[Random.Range(0, 4)];
            }
            else
            {
                target = myTargets[Random.Range(0, myTargets.Count)];
            }
        }
        if (target != null && !target.Alive)
        {
            myTargets.Remove(target);
            target = null;
        }
    }

    protected virtual void Shoot()
    {
        myAnimator.Play("Attack");
        myAnimator.AnimationCompleted = CompletedDelegate;

        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();

        projectile.GetComponent<Transform>().SetParent(projectileParent);
        projectile.transform.position = transform.position;

        projectile.Initialize(this);
    }

    public void CompletedDelegate(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
    {
        if(target == null)
        {
            myAnimator.Play("Idle");
            myAnimator.AnimationCompleted = null;
        }       
    }

    public void Upgrade()
    {
        GameManager.Instance.Currency -= upgrade.Price;
        upgrade.Price += addUpgradePrice;

        damage += upgrade.Damage;
        upgradeLevel++;
    }

    public string GetStat()
    {
        return string.Format("Level: {0}\nDamage: {1}", upgradeLevel, damage);
    }
}
