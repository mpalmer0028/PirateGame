using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject BallPrefab;    
    public Transform BallInstantiateTransform;    

    private GameObject Ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.Space) && Ball == null){
            Ball = FireCannonBall();
            Destroy(Ball, 2);
        }
    }

    public GameObject FireCannonBall(){
        var ball =  Instantiate(BallPrefab, BallInstantiateTransform.position, BallInstantiateTransform.rotation);
        var rb = ball.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.up*10000, ForceMode.Force);
        return ball;
    }
}
