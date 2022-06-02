using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TrooperAI : GroundAI
{

    public Vector3 guardPos { get; protected set; }
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
        }
        else
        {
            aimDir = transform.right;
            isAimPressed = false;
        }

        float speed = 1f;

        if (_attentionSpan < 0)
        {
            targetPos = guardPos;
            speed = 0.25f;
        }

        Vector2 dir = targetPos - transform.position;
        if (controller.CheckLedge(dir.x) || math.abs(dir.x) <= targetReachedTreshold.x)
        {
            movementInput = Vector2.zero;
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
}
