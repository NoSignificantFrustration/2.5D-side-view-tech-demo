using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TrooperAI : GroundAI
{

    public Vector3 guardPos { get; protected set; }
    public bool guardDirection { get; protected set; }

    [field: SerializeField] public Transform aimPivot { get; protected set; }
    public EntityBase target { get; protected set; }
    public Vector3 targetPos { get; protected set; }
    [field: SerializeField] public SightPerception sight { get; protected set; }
    [field: SerializeField, Min(0.1f)] public float lookCooldown { get; protected set; }
    public float _lookCooldown { get; protected set; }
    [field: SerializeField, Min(1f)] public float attentionSpan { get; protected set; }
    public float _attentionSpan { get; protected set; }
    public bool canSeeTarget { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        guardPos = transform.position;
        guardDirection = controller.facingRight;
        aiState = AIState.Guarding;
    }
    public override void EvaluateActions()
    {
        if (_lookCooldown < 0f)
        {
            FindTarget();
        }


        if (canSeeTarget)
        {
            isAimPressed = true;

            targetPos = target.transform.position;
            aimPos = targetPos;
            aimDir = ((Vector2)aimPivot.position - aimPos).normalized;

            aiState = AIState.Chasing;
        }
        else
        {
            aimDir = transform.right;
            isAimPressed = false;
        }

        float speed = 1f;



        if (_attentionSpan < 0)
        {
            aiState = AIState.Returning;
            targetPos = guardPos;
            speed = 0.25f;
        }

        float currentTargetReachedTreshold = targetReachedTreshold.x;
        if (aiState == AIState.Returning)
        {
            currentTargetReachedTreshold = 0.2f;
        }

        Vector2 dir = targetPos - transform.position;
        if (controller.CheckLedge(dir.x) )
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

    public override void TimerUpdate()
    {
        _lookCooldown -= Time.deltaTime;
        _attentionSpan -= Time.deltaTime;
    }

    protected override void FindTarget()
    {

        sight.Look(self, out List<EntityBase> entities);

        float smallsetDistance = float.MaxValue;
        EntityBase closestTarget = null;

        foreach (EntityBase item in entities)
        {
            if (item.faction != self.faction)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if (distance < smallsetDistance)
                {
                    smallsetDistance = distance;
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
}
