using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [field: SerializeField] public Transform gunTransform { get; protected set; }
    [SerializeField] private GameObject bulletPrefab;
    public EntityBase self { get; protected set; }


    public virtual void Awake()
    {
        self = GetComponent<EntityBase>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Fire()
    {

    }
    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(gunTransform.position, gunTransform.right * 100f);
        
    }
}
