using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SightPerception : MonoBehaviour
{

    [field: SerializeField, Min(0f)] public float viewDistance { get; protected set; }
    [field: SerializeField, Range(0f, 360f)] public float fieldOfView { get; protected set; }
    [field: SerializeField] public Vector2 positionOffset { get; protected set; }
    [field: SerializeField] public bool showDebug { get; protected set; }
    public LayerMask entityMask { get; protected set; }
    public LayerMask groundMask { get; protected set; }


    private void Awake()
    {
        entityMask = LayerMask.GetMask("Entity");
        groundMask = LayerMask.GetMask("Ground");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Look(EntityBase self, out List<EntityBase> entities)
    {
        entities = new List<EntityBase>();
        Collider[] results = Physics.OverlapSphere(transform.position + (Vector3)positionOffset, viewDistance, entityMask);
        Vector3 pos = transform.position + (Vector3)positionOffset;

        

        if (results.Length > 0)
        {
            foreach (Collider item in results)
            {

                if (item.gameObject.TryGetComponent<EntityBase>(out EntityBase comp))
                {
                    if (comp != self)
                    {
                        if (Vector3.Angle(item.transform.position - pos, transform.right) < fieldOfView)
                        {
                            if (!Physics.Raycast(pos, (item.transform.position - pos).normalized, Vector3.Distance(pos, item.transform.position), groundMask))
                            {
                                entities.Add(comp);
                            }
                            
                        }
                    }
                    
                    
                }
            }
            if (entities.Count == 0)
            {
                return false;
            }
            return true;
        }

        return false;
    }

    public bool CheckLineOfSight(Vector3 endPos)
    {
        Vector3 pos = transform.position + (Vector3)positionOffset;

        return !Physics.Raycast(pos, (endPos - pos).normalized, Vector3.Distance(pos, endPos), groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebug) return;
        
        Vector3 pos = transform.position + (Vector3)positionOffset;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(pos, viewDistance);
        float radAngle = math.radians(fieldOfView);

        float x = viewDistance * math.cos(radAngle);
        float y = viewDistance * math.sin(radAngle);

        Vector3 dir1 = new Vector3(x, y, 0f);
        dir1 = Quaternion.Euler(transform.rotation.eulerAngles) * dir1;
        Vector3 end1 = pos + dir1;

        Vector3 dir2 = new Vector3(x, -y, 0f);
        dir2 = Quaternion.Euler(transform.rotation.eulerAngles) * dir2;
        Vector3 end2 = pos + dir2;

        Gizmos.DrawLine(pos, end1);
        Gizmos.DrawLine(pos, end2);

        if (!Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.blue;

        List<EntityBase> entities;
        Look(null, out entities);

        foreach (EntityBase item in entities)
        {
            Gizmos.DrawLine(pos, item.transform.position);
        }
    }
}
