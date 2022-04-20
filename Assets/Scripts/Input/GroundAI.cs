using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundAI : InputBase
{

    [field: SerializeField] public virtual Vector2 targetReachedTreshold { get; protected set; }
    public GraphPathfindingAgent pathfindingAgent { get; protected set; }
    public PathNode nextNode { get; protected set; }

    public GroundController controller { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<GroundController>();
    }
}
