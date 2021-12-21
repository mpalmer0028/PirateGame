using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    private PlayerMovementScript PMS;
    //public int MaskIndex;

    // Start is called before the first frame update
    void Start()
    {
        PMS = GetComponent<PlayerMovementScript>();
    }

    void FixedUpdate()
    {
        var hit = GetVehicleUnderPlayer();
        PMS.DistanceTooVehicle = hit.distance;
        PMS.VehiclePoint = hit.point;
        //Debug.Log(hit.distance);
        PMS.Vehicle = hit.collider ? hit.collider.gameObject : null;
        
        
    }

    public RaycastHit GetVehicleUnderPlayer(){
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, .6f, 1 << 6);
        return hit;
    }
}
