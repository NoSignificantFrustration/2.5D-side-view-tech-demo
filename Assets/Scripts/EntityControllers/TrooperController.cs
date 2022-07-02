using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrooperController : GroundController
{
    [SerializeField] private GameObject weaponPivot;
    [field: SerializeField] public RangedWeapon rangedWeapon { get; protected set; }



    protected virtual void OnEnable()
    {
        rangedWeapon.self = input.self;
    }


    protected override void Update()
    {
        base.Update();
        input.TimerUpdate();
        if (input.isAimPressed)
        {
            weaponPivot.transform.localRotation = Quaternion.FromToRotation(transform.right.x > 0 ? Vector2.left : Vector2.right, transform.right.x > 0 ? input.aimDir : new Vector2(input.aimDir.x, input.aimDir.y * -1));
            if (input.isFirePressed)
            {
                rangedWeapon.Attack();
            }
        }
        else
        {
            weaponPivot.transform.rotation = transform.rotation;
        }
        
    }
}
