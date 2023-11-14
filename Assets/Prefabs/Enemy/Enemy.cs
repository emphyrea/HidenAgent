using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBTTaskInterface, ITeamInterface, IMovementInterface
{
    [SerializeField] ValueGauge healthBarPrefab;
    [SerializeField] Transform healthBarAttachTransform;
    [SerializeField] int teamID = 2;

    HealthComponent healthComponent;
    MovementComponent movementComponent;
    [SerializeField] DamageComponent damageComponent;
    ValueGauge healthBar;
    Animator animator;
    NavMeshAgent agent;

    Vector3 prevPos;
    Vector3 velocity;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.onTakenDamage += TookDamage;
        healthComponent.onHealthEmpty += StartDeath;
        healthComponent.onHealthChange += HealthChanged;

        damageComponent.SetTeamInterface(this);

        healthBar = Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform);
        UIAttachComponent attachmentComp = healthBar.AddComponent<UIAttachComponent>();
        attachmentComp.SetupAttachment(healthBarAttachTransform);

        movementComponent = GetComponent<MovementComponent>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public int GetTeamID() { return teamID; }

    private void HealthChanged(float currentHealth, float delta, float maxHealth)
    {
        healthBar.SetValue(currentHealth, maxHealth);
    }

    private void StartDeath(float delta, float maxHealth)
    {
        Destroy(healthBar.gameObject);

        animator.SetTrigger("death");

        GetComponent<AIController>().StopAILogic();
    }



    private void TookDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        Debug.Log($"took damage {delta}, now health is: {currentHealth}/{maxHealth}");

    }

    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();
    }

    private void CalculateVelocity()
    {
        velocity = (transform.position - prevPos) /Time.deltaTime;
        prevPos = transform.position;
        animator.SetFloat("speed", velocity.magnitude);
    }

    public void RotateTowards(Vector3 direction)
    {
        movementComponent.RotateTowards(direction);
    }

    public void RotateTowards(GameObject target)
    {
        movementComponent.RotateTowards(target.transform.position - transform.position);
    }

    public void AttackTarget(GameObject target)
    {
        animator.SetTrigger("attack");
    }

    public void AttackPoint()
    {
        damageComponent.DoDamage();
    }

    public void DeathAnimFinished()
    {
        Destroy(gameObject);
    }

    public float GetMoveSpeed()
    {
        return agent.speed;
    }

    public void SetMoveSpeed(float speed)
    {
        agent.speed = speed;
    }
}
