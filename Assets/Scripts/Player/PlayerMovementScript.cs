using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    /// <summary>
    /// Custom setter for toggling movement system
    /// </summary>
    [SerializeField]
    private GameObject _Vehicle;
    public GameObject Vehicle {
        get { 
            return _Vehicle;
        }
        set {
            _Vehicle = value;
            if(value){
                if(GetComponent<Rigidbody>()) {
                    Destroy(gameObject.GetComponent<Rigidbody>());
                    //Debug.Log(GetComponent<Rigidbody>());
                }
               transform.SetParent(value.transform);
            } else if(!GetComponent<Rigidbody>()){
                transform.SetParent(null);
                this.AddRigidbody();
            }
        }
    }

    /// <summary>
    /// Walk speed on land
    /// </summary>
    public float MoveSpeed;

    //public float RotateSpeed;
    /// <summary>
    /// Camera rotation speed horizontal
    /// </summary>
    public float CameraSpeedH = 2.0f;

    /// <summary>
    /// Camera rotation speed vertical
    /// </summary>
    public float CameraSpeedV = 2.0f;

    /// <summary>
    /// How much you can look up and down
    /// </summary>
    public float MaxMinCameraAngleX = 45f;

    /// <summary>
    /// Y axis rotation tracker
    /// </summary>
    private float Yaw = 0.0f;

    /// <summary>
    /// X axis rotation tracker
    /// </summary>
    private float Pitch = 0.0f;
    
    private Rigidbody RB;
    private GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        if(Vehicle) {
            Destroy(GetComponent<Rigidbody>());
            transform.SetParent(Vehicle.transform);
        } else {
            AddRigidbody();
        }
        
        Camera = transform.Find("Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        //Debug.Assert(_Vehicle == Vehicle);
        if(Vehicle){
            
        } else if(RB) {
            // Track camera rotation from mouse
            Yaw += CameraSpeedH * Input.GetAxis("Mouse X");
            Pitch -= CameraSpeedV * Input.GetAxis("Mouse Y");

            // Limit angle
            Pitch = Pitch > MaxMinCameraAngleX ? MaxMinCameraAngleX : Pitch;    
            Pitch = Pitch < -MaxMinCameraAngleX ? -MaxMinCameraAngleX : Pitch;    

            var cameraVector = new Vector3(0.0f, Yaw, 0.0f);

            // Move player
            var inputVector = new Vector3(Input.GetAxis("Horizontal")*MoveSpeed,0,Input.GetAxis("Vertical")*MoveSpeed);
            RB.AddRelativeForce(inputVector,ForceMode.Force);
            // Rotate player
            RB.rotation = Quaternion.Euler(RB.rotation.eulerAngles.x, Yaw, RB.rotation.eulerAngles.z);
            
            // Move Camera
            Camera.transform.localRotation = Quaternion.Euler(Pitch,0,0);

            //RB.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*RotateSpeed,0),ForceMode.Force);
        }
    }
        
    // is called by Unity when ever a value in the inspector is changed
    private void OnValidate()
    {
        // Make sure editor updates property
        Vehicle = _Vehicle;
    }

    /// <summary>
    /// Add rigidbody with settings to player
    /// </summary>
    /// <returns></returns>
    public Rigidbody AddRigidbody(){
        this.RB = GetComponent<Rigidbody>();
        if(!this.RB){
            this.RB = gameObject.AddComponent<Rigidbody>();
        }
        // Add any Rigidbody settings here 
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        return this.RB;
    }
}
