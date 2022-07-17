using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//[RequireComponent(typeof(TrooperController))]
public class TrooperAI : GroundAI
{

    public Vector3 guardPos { get; protected set; }
    public bool guardDirection { get; protected set; }

    [field: SerializeField] public Transform aimPivot { get; protected set; }
    public EntityBase target { get; protected set; }
    public Vector3 targetPos { get; protected set; }
    public TrooperController trooperController { get; protected set; }
    [field: SerializeField] public SightPerception sight { get; protected set; }
    [field: SerializeField, Min(0.1f)] public float lookCooldown { get; protected set; }
    public float _lookCooldown { get; protected set; }
    [field: SerializeField, Min(1f)] public float attentionSpan { get; protected set; }
    public float _attentionSpan { get; protected set; }
    public bool canSeeTarget { get; protected set; }
    [field: SerializeField, Min(0f)] public float spookTime { get; protected set; }
    public float _spookTime { get; protected set; }


    protected virtual void OnEnable()
    {
        self.onDamageReceivedEvent += OnHit;
    }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<TrooperController>();
        guardPos = transform.position;
        guardDirection = transform.rotation.eulerAngles.y == 0;
        //guardDirection = controller.facingRight;
        aiState = AIState.Guarding;

        if (controller is TrooperController ctrl)
        {
            trooperController = ctrl;
        }
        else
        {
            Debug.LogError(gameObject.name + " lacks a TrooperController!");
        }

    }
    public override void EvaluateActions()
    {
        if (_lookCooldown < 0f)
        {
            FindTarget();
        }

        bool isFacingLedge = controller.CheckLedge(transform.right.x);

        if (aiState == AIState.Spooked && _spookTime > 0f)
        {

        }
        else if (canSeeTarget)
        {
            targetPos = target.transform.position;
            if (aiState == AIState.Guarding || aiState == AIState.Returning)
            {
                aiState = AIState.Spooked;
                _spookTime = spookTime;
            }
            else
            {

                if (aiState == AIState.Spooked || aiState == AIState.Searching)
                {
                    aiState = AIState.Chasing;
                }

                aimPos = targetPos;
                aimDir = ((Vector2)aimPivot.position - aimPos).normalized;


                float distanceToTarget = Vector2.Distance(transform.position, targetPos);

                if (aiState == AIState.Chasing)
                {
                    if (distanceToTarget < trooperController.rangedWeapon.maxAttackRange * trooperController.rangedWeapon.preferredAttackRange)
                    {
                        aiState = AIState.Attacking;
                    }
                    else if (isFacingLedge)
                    {
                        aiState = AIState.Attacking;
                    }

                }
                else if (aiState == AIState.Attacking)
                {
                    if (distanceToTarget > trooperController.rangedWeapon.maxAttackRange && !isFacingLedge)
                    {
                        aiState = AIState.Chasing;
                    }
                }
                else
                {
                    aiState = AIState.Chasing;
                }
            }

            
            


        }
        else
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPos);

            isFirePressed = false;
            aimDir = transform.right;
            isAimPressed = false;
            if (aiState == AIState.Spooked)
            {
                aiState = AIState.Chasing;
            }
            else if (aiState == AIState.Attacking)
            {
                aiState = AIState.Chasing;
            }
            else if (aiState == AIState.Chasing && distanceToTarget < targetReachedTreshold.x)
            {
                aiState = AIState.Searching;
                targetPos = new Vector3(targetPos.x + (controller.facingRight ? 10f : -10f), targetPos.y, targetPos.z);
            }
        }

        //float speed = 1f;



        if (_attentionSpan < 0 && aiState != AIState.Guarding)
        {
            aiState = AIState.Returning;
            targetPos = guardPos;
            //speed = 0.25f;
        }


        //Debug.Log(aiState);
        switch (aiState)
        {
            case AIState.Guarding:
                movementInput = Vector2.zero;
                isAimPressed = false;
                isFirePressed = false;
                break;
            case AIState.Chasing:
                isAimPressed = false;
                isFirePressed = false;
                EvaluateMovement();
                break;
            case AIState.Attacking:
                movementInput = Vector2.zero;
                isAimPressed = true;
                isFirePressed = true;
                break;
            case AIState.Returning:
                isAimPressed = false;
                isFirePressed = false;
                EvaluateMovement();
                break;
            case AIState.Searching:
                isAimPressed = false;
                isFirePressed = false;
                EvaluateMovement();
                break;
            default:
                break;
        }
    }

    protected virtual void OnHit(Damage damage)
    {
        //if (damage.causer == null) return;
        if (aiState == AIState.Attacking || canSeeTarget) return;
        

        _attentionSpan = attentionSpan;

        Vector3 pos = transform.position;

        float xDiff = pos.x - damage.source.transform.position.x;
        float targetDir = xDiff > 0f ? 1f : -1f;

        targetPos = new Vector3(pos.x + targetDir * 10f, pos.y, 0f);
        if (aiState == AIState.Guarding || aiState == AIState.Returning)
        {
            aiState = AIState.Spooked;
            _spookTime = spookTime;
        }

    }

    public override void TimerUpdate()
    {
        _lookCooldown -= Time.deltaTime;
        _attentionSpan -= Time.deltaTime;
        _spookTime -= Time.deltaTime;
    }

    protected override void FindTarget()
    {

        sight.Look(self, out List<EntityBase> entities);

        float smallestDistance = float.MaxValue;
        EntityBase closestTarget = null;

        foreach (EntityBase item in entities)
        {
            if (item.faction != self.faction)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestTarget = item;
                }
            }
        }



        if (closestTarget == null && target != null)
        {
            if (!sight.CheckLineOfSight(target.transform.position))
            {
                target = null;
            }
        }
        else
        {
            target = closestTarget;
        }

        if (target != null)
        {
            canSeeTarget = true;
            _attentionSpan = attentionSpan;
            targetPos = target.transform.position;
        }
        else
        {
            canSeeTarget = false;
            target = null;
        }



    }

    public override void EdgeReached()
    {
        movementInput = new Vector2(0f, movementInput.y);

    }

    public override void EvaluateMovement()
    {
        float currentTargetReachedTreshold = targetReachedTreshold.x;
        if (aiState == AIState.Returning)
        {
            currentTargetReachedTreshold = 0.2f;
        }
        Vector2 dir = targetPos - transform.position;

        if (controller.CheckLedge(dir.x))
        {
            movementInput = Vector2.zero;
        }
        else if (math.abs(dir.x) <= currentTargetReachedTreshold)
        {
            if (aiState == AIState.Returning)
            {
                if (guardDirection != controller.facingRight)
                {
                    movementInput = new Vector2(movementInput.x / 1000f * -1, 0f);
                }
                else
                {
                    movementInput = Vector2.zero;
                    aiState = AIState.Guarding;
                }
            }
            else
            {
                movementInput = Vector2.zero;
            }

        }
        else
        {

            float speed = 1f;

            if (aiState == AIState.Returning)
            {
                speed = 0.25f;
            }


            float movx = 0f;
            float movy = 0f;
            if (dir.x > 0)
            {
                movx = 1;
            }
            else if (dir.x < 0)
            {
                movx = -1;
            }

            if (dir.y > 0)
            {
                movy = 1;
            }
            else if (dir.y < 0)
            {
                movy = -1;
            }

            movementInput = new Vector2(movx, movy) * speed;
        }
    }

    protected virtual void OnDisable()
    {
        self.onDamageReceivedEvent -= OnHit;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(targetPos, 1f);
    //}
}

