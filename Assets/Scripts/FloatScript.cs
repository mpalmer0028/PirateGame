using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScript : MonoBehaviour
{
    public GameObject[] FloatPoints;

    public float SubmergedDepth = 3f; 
    public float Displacement = 3f; 

    private Rigidbody RB;
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var maxBuoyancy = -Physics.gravity.y/FloatPoints.Length;
        foreach(var fp in FloatPoints){
            RaycastHit hit;
            if(Physics.Raycast(fp.transform.position+new Vector3(0,SubmergedDepth*3,0), Vector3.down, out hit, SubmergedDepth*3, 1 << 4)){
                var buoyancy = Mathf.Clamp01(SubmergedDepth*3-hit.distance)*Displacement;
                // Debug.Log(buoyancy);
                RB.AddForceAtPosition(new Vector3(0, maxBuoyancy * buoyancy,0), fp.transform.position,ForceMode.Force);
            }
        }
        
    }
}
