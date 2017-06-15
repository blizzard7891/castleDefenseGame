using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Monster : MonoBehaviour {

    public float speed;
    public int damage;
    public float attackCoolTime;
    [SerializeField]
    protected Stat health;
    [SerializeField]
    protected int HP;
    [SerializeField]
    protected int monsterCurrency;
    [SerializeField]
    protected string attackSoundName;

    private GameObject target;

    protected tk2dSpriteAnimator myAnimator;
    public tk2dSprite tmp;

    protected bool canAttack = true;
    protected bool isAttack = false;
    protected bool isSpawn;
    public bool isActive;
    private float attackTimer;

    public bool Alive
    {
        get { return health.CurrentVal > 0; }
    }

    public float Y
    {
        get { return tmp.GetBounds().size.y; }
    }

    protected virtual void Awake()
    {
        isActive = false;
        health.Initialize();
        myAnimator = GetComponent<tk2dSpriteAnimator>();
        tmp = GetComponent<tk2dSprite>();
    }

    // Use this for initialization
    protected virtual void Start () {
        target = GameObject.FindWithTag("Castle");
    }
	
    protected virtual void FixedUpdate()
    {
        if (!isSpawn)
        {
            if (!isAttack && isActive)
            {
                MoveToTarget();
            }
            else if (isAttack && isActive)
            {
                Attack();
            }
        }
    }

	// Update is called once per frame
	protected virtual void Update () {
        
    }

    public void MoveToTarget()
    {
        if (target != null && isActive)
        {
            if (!myAnimator.IsPlaying("Walk"))
            {
                myAnimator.Play("Walk");
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        }
    }

    public void Spawn()
    {
        isSpawn = true;

        this.health.Bar.Reset();
        this.health.MaxVal = HP;
        this.health.CurrentVal = this.health.MaxVal;

        int spawnIndex = Random.Range(0, LevelManager.Instance.SpawnPoints.Length);
        transform.position = LevelManager.Instance.SpawnPoints[spawnIndex].transform.position;

        isActive = true;

        myAnimator.Play("Spawn");
        StartCoroutine(Spawning());
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
        else if (canAttack && target != null && isAttack && isActive)
        {
            target.GetComponent<Castle>().TakeDamage(damage);
            canAttack = false;
            SoundManager.Instance.PlaySFX(attackSoundName);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isActive)
        {
            health.CurrentVal -= damage;
            SoundManager.Instance.PlaySFX("Splat");

            if (health.CurrentVal <= 0)
            {
                GameManager.Instance.TmpCurrency += monsterCurrency;

                myAnimator.Play("Die");
                isActive = false;

                StartCoroutine(Dieing());

                GetComponent<tk2dSprite>().SortingOrder--;

                SoundManager.Instance.PlaySFX("monsterDie");
            }
        }
    }

    public void Release()
    {
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveMonster(this);

        GetComponent<tk2dSprite>().SortingOrder++;

        isActive = false;
    }

    public IEnumerator Spawning()
    {
        float progress = 0;

        while (progress <= 1.5)
        {
            progress += Time.deltaTime * 1;
            yield return null;
        }

        isSpawn = false;
    }

    public IEnumerator Dieing()
    {
        float progress = 0;

        while (progress <= 1.5)
        {
            progress += Time.deltaTime * 1;
            yield return null;
        }

        Release();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Castle")
        {
            myAnimator.Play("Attack");
            isAttack = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Castle")
        {
            myAnimator.Play("Walk");
            isAttack = false;
        }
    }
}
