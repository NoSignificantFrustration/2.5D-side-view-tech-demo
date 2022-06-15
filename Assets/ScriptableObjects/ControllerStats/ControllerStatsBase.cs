using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "GroundControllerStat", menuName = "Scriptable Objects/Controller Stats")]
public abstract class ControllerStatsBase : ScriptableObject
{
    [field: SerializeField] public float speed { get; protected set; }
    [field: SerializeField] public float acceleration { get; protected set; }
    [field: SerializeField] public float baseGravity { get; protected set; }
}
