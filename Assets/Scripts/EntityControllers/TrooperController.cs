using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrooperController : GroundController
{
    [SerializeField] private GameObject weapon;

    protected override void Update()
    {
        base.Update();
        input.TimerUpdate();
        if (input.isAimPressed)
        {
            weapon.transform.localRotation = Quaternion.FromToRotation(transform.right.x > 0 ? Vector2.left : Vector2.right, transform.right.x > 0 ? input.aimDir : new Vector2(input.aimDir.x, input.aimDir.y * -1));
        }
        else
        {
            weapon.transform.rotation = transform.rotation;
        }
        
    }
}
