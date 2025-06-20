using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum MonsterState
{
    Idle,
    Walking,
    Running,
    Attacking
}

public class MonsterController : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform targetPlayer;

    public float detectionRange = 10f;
    public float attackRange = 1.5f;

    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 8.0f;

    private MonsterState currentState = MonsterState.Idle;
    private MonsterSFXManager sfx;

    void Start()
    {
        sfx = GetComponent<MonsterSFXManager>();
        TransitionToState(MonsterState.Idle);
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                if (IdleAnimationEnded())
                    TransitionToState(MonsterState.Walking);
                break;

            case MonsterState.Walking:
                Patrol();

                if (PlayerDetected())
                    TransitionToState(MonsterState.Running);
                break;

            case MonsterState.Running:
                ChasePlayer();
                if (LostPlayer())
                    TransitionToState(MonsterState.Idle);
                break;

            case MonsterState.Attacking:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    animator.SetTrigger("Attack");
                    agent.isStopped = true;
                    StartCoroutine(AttackCooldown());
                }
                break;
        }

         
        if (currentState != MonsterState.Attacking && IsPlayerInAttackRange())
        {
            TransitionToState(MonsterState.Attacking);
        }
    }

    void TransitionToState(MonsterState newState)
    {
        animator.ResetTrigger("Attack");
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);

        sfx?.StopSound();

        switch (newState)
        {
            case MonsterState.Walking:
                animator.SetBool("IsWalking", true);
                agent.isStopped = false;
                agent.speed = walkSpeed;
                SetRandomDestination();
                sfx?.PlayWalkSound();
                break;

            case MonsterState.Running:
                animator.SetBool("IsRunning", true);
                agent.isStopped = false;
                agent.speed = runSpeed;
                sfx?.PlayRunSound();
                break;

            case MonsterState.Attacking:
                agent.isStopped = true;
                animator.SetTrigger("Attack");
                sfx?.PlayAttackSound();
                break;

            case MonsterState.Idle:
                agent.isStopped = true;
                sfx?.PlayIdleSound();
                break;
        }

        currentState = newState;
    }


    IEnumerator AttackCooldown()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 게임 오버
        EscapeManager.currentEscape = EscapeManager.EscapeType.None;
        SceneManager.LoadScene("EndingScene");
    }




    void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * 10f;
        randomDir += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void ChasePlayer()
    {
        if (targetPlayer != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetPlayer.position);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TransitionToState(MonsterState.Attacking);
        }
    }

    bool PlayerDetected()
    {
        if (targetPlayer == null) return false;
        float dist = Vector3.Distance(transform.position, targetPlayer.position);
        return dist < detectionRange;
    }

    bool LostPlayer()
    {
        if (targetPlayer == null) return true;
        float dist = Vector3.Distance(transform.position, targetPlayer.position);
        return dist > detectionRange * 1.5f;
    }

    bool IdleAnimationEnded()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }

    bool IsPlayerInAttackRange()
    {
        if (targetPlayer == null) return false;
        float dist = Vector3.Distance(transform.position, targetPlayer.position);
        return dist <= attackRange;
    }

    public void OnPlayerSeen(Transform player)
    {
        if (currentState == MonsterState.Idle || currentState == MonsterState.Walking)
        {
            Debug.Log("Found Player");
            targetPlayer = player;
            TransitionToState(MonsterState.Running);
        }
    }

    public bool IsChasing()
    {
        return currentState == MonsterState.Running || currentState == MonsterState.Attacking;
    }
}
