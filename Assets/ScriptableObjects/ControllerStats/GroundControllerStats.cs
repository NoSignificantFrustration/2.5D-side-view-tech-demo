using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundControllerStat", menuName = "Scriptable Objects/Controller Stats/Ground Controller Stats")]
public class GroundControllerStats : ControllerStatsBase
{
    [field: SerializeField] public float jumpForce { get; protected set; }
    [field: SerializeField] public float jumpDuration { get; protected set; }
    [field: SerializeField] public float fallGravityMultiplier { get; protected set; }
    [field: SerializeField, Range(0.7f, 1f)] public float idleSpeedMultiplier { get; protected set; }
}
