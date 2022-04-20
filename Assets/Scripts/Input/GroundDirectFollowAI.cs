using Unity.Mathematics;
using UnityEngine;

public class GroundDirectFollowAI : GroundAI
{

    [SerializeField] public Transform targetEntity;
    public override void EvaluateMovement()
    {
        Vector2 dir =  targetEntity.position - transform.position;
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

            movementInput = new Vector2(movx, movy);
        }
    }
}
