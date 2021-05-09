using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour, IEnemy
{
    public enum EnemyType { ICECREAM, PIZZA, BURGER, DONUT, FRIES };

    protected ObjectPooler objectPooler;
    protected AudioManager audioManager;
    protected Animator animator;

    public EnemyStats stats = new EnemyStats();

    public EnemyType enemyType;

    [HideInInspector]
    public PlayerController player;

    private Rigidbody2D rigidBody;

    public bool shoot = false;
    protected bool waitToShoot;

    private Vector2 lookDir = new Vector2(0, -1);

    public GameObject deathEffect;

    [SerializeField]
    private HPIndicatorUI hpIndicator;

    [SerializeField]
    private string collideSound = "EnemyCollide";

    private void OnEnable()
    {
        stats.Init();
        player = FindObjectOfType<PlayerController>();

        if (hpIndicator != null)
        {
            hpIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }
    }

    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioManager = AudioManager.instance;
        objectPooler = ObjectPooler.instance;
    }
    
    protected void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector2 temp = (player.transform.position - transform.position);
        Vector2 movementDirection = temp.normalized;

        if (!Mathf.Approximately(movementDirection.x, 0.0f) || !Mathf.Approximately(movementDirection.y, 0.0f))
        {
            lookDir.Set(movementDirection.x, movementDirection.y);
            lookDir.Normalize();
        }

        if (animator != null)
        {
            animator.SetFloat("Look X", lookDir.x);
            animator.SetFloat("Look Y", lookDir.y);
            
            animator.SetFloat("Speed", movementDirection.magnitude);
        }

        if (temp.magnitude > stats.movementRange || enemyType == EnemyType.BURGER)
        {
            Vector2 position = rigidBody.position;
            
            position.x += movementDirection.x * stats.speed * Time.fixedDeltaTime;
            position.y += movementDirection.y * stats.speed * Time.fixedDeltaTime;

            rigidBody.MovePosition(position);
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    public void TakeHit(int damage)
    {
        stats.CurrentHealth -= damage;

        if (stats.CurrentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (hpIndicator != null)
        {
            hpIndicator.SetHealth(stats.CurrentHealth, stats.maxHealth);
        }
    }

    public float GetArmorStat()
    {
        return stats.armorPct;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null && !player.IsInvincible())
            {
                audioManager.PlaySound(collideSound);
                player.DamagePlayer(stats.damage);
            }
        }
    }

    protected void SetPosRotActivate(GameObject obj, Vector2 pos, Quaternion rot)
    {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
    }
}
