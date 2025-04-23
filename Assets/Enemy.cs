using DG.Tweening;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Events;

public enum EnemyPursueGoal
{
    PLAYER, SHRINE
}

public class Enemy : MonoBehaviour, IDamage
{
    protected NavMeshAgent agent;
    [SerializeField] Renderer model;
    Outliner display;
    protected Animator animator;
    protected string currentAnimationState;

    public int health = 20;
    int maxHealth;
    public float moveSpeed = 20.0f;

    [SerializeField] int faceTargetSpeed;

    protected Vector3 goalLocation;

    Color normalColor;

    [SerializeField] ParticleSystem hitParticles;

    protected Vector3 lookDir;

    int preferredRoute;

    [Range(0.0f, 3.0f)]
    [SerializeField] float shrinePreference;
    [SerializeField] float aggressionThreshhold = 5.0f;
    float focus = 0.0f;
    [SerializeField] float maxDisengageTime = 7.5f;
    Coroutine disengagementRoutine;
    protected EnemyPursueGoal currentPursue;
    [SerializeField] protected EnemyPursueGoal originalPursue = EnemyPursueGoal.SHRINE;

    public Transform headPos;
    [SerializeField] float fov = 75.0f;

    protected UnityEvent<EnemyPursueGoal> pursueUpdating; 
    void Awake()
    {
        pursueUpdating = new UnityEvent<EnemyPursueGoal>();
    }

    protected void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        //agent.updateRotation = false;
        animator = GetComponent<Animator>();
        display = model.GetComponent<Outliner>();
        normalColor = display.backingColor;
        maxHealth = health;
        currentPursue = originalPursue;
        GetNewTarget();
    }

    void Update()
    {
        focus -= (Time.deltaTime * shrinePreference);
        if (focus >= aggressionThreshhold && currentPursue != EnemyPursueGoal.PLAYER)
        {
            EnemyPursueGoal prevPursue = currentPursue;
            currentPursue = EnemyPursueGoal.PLAYER;
            pursueUpdating.Invoke(prevPursue);
            SetAgentAreaCost(preferredRoute, 1.1f);
            GetNewTarget();
        } else if (focus < aggressionThreshhold && currentPursue == EnemyPursueGoal.PLAYER)
        {
            EnemyPursueGoal prevPursue = currentPursue;
            currentPursue = EnemyPursueGoal.SHRINE;
            pursueUpdating.Invoke(prevPursue);
            SetAgentAreaCost(preferredRoute, 1);
            GetNewTarget();
        }
        lookDir = goalLocation - transform.position;
        Behavior();
    }

    protected virtual void Behavior() { }
    protected virtual void Death() {
        WaveManager.instance.MobDeath();
        Destroy(gameObject);
    }
    protected void ChangeAnimationState(string newState)
    {
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.1f);
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        focus += amount;
        if (disengagementRoutine != null)
        {
            StopCoroutine(disengagementRoutine);
            disengagementRoutine = StartCoroutine(Disengage());
        }
        flashRed();
        ParticleSystem particles = Instantiate(hitParticles, model.transform);
        particles.Play();
        if (health <= 0)
        {
            Death();
        }
    }

    void flashRed()
    {
        DOTween.To(() => display.backingColor, x => display.backingColor = x, Color.red, 0.05f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            DOTween.To(() => display.backingColor, x => display.backingColor = x, normalColor, 0.05f).SetEase(Ease.OutExpo);
        });
    }

    protected void faceTarget(Vector3 target)
    {
        target.y = 0;
        if (target ==Vector3.zero)
        {
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(target.normalized);

        Quaternion floatRot = Quaternion.Euler(0, targetRot.eulerAngles.y, 0); 
        transform.rotation = Quaternion.Lerp(transform.rotation, floatRot, Time.deltaTime * faceTargetSpeed);
    }

    public void SetAgentAreaCost(int layer, float cost)
    {
        agent.SetAreaCost(layer, cost);
        preferredRoute = layer;
    }

    IEnumerator Disengage()
    {
        yield return new WaitForSeconds(maxDisengageTime);
        focus = 0.0f;
        disengagementRoutine = null;
    }

    public virtual void GetNewTarget()
    {
        if (currentPursue == EnemyPursueGoal.PLAYER)
        {
            goalLocation = gameManager.instance.player.transform.position;
        }
        else {
            goalLocation = gameManager.instance.GetShrineLocationDispersed();
        }
    }

    bool CanSeeTarget(string tag)
    {
        Vector3 dir = goalLocation - transform.position;
        float angleToPlayer = Vector3.Angle(new Vector3(dir.x, 0, dir.z), transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, dir, out hit))
        {
            if (hit.collider.CompareTag(tag) && angleToPlayer <= fov)
            {
                return true;
            }
        }
        return false;
    }
}
