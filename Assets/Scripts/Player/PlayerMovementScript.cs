using System;
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
               VehicleWalkMode();
            } else if(!GetComponent<Rigidbody>()){
                transform.SetParent(null, true);
                this.LandWalkMode();
            }
        }
    }

    /// <summary>
    /// Walk speed on land
    /// </summary>
    public float MoveSpeed;

    /// <summary>
    /// On vehicle walk speed on land
    /// </summary>
    public float OnVehicleWalkSpeed;

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
    
    private BoxCollider BC;
    private Rigidbody RB;
    private GameObject Camera;
    private GameObject VehicleRotationCorrector;
    private CharacterController CC;
    public float DistanceTooVehicle;

    // Start is called before the first frame update
    void Start()
    {
        VehicleRotationCorrector = transform.Find("VehicleRotationCorrector").gameObject;
        Camera = VehicleRotationCorrector.transform.Find("Camera").gameObject;
        BC =  GetComponent<BoxCollider>();
        CC =  GetComponent<CharacterController>();

        if(Vehicle) {
            VehicleWalkMode();
            
        } else {
            LandWalkMode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate()
    {
        // Track camera rotation from mouse
        Yaw += CameraSpeedH * Input.GetAxis("Mouse X");
        Pitch -= CameraSpeedV * Input.GetAxis("Mouse Y");

        // Limit angle
        Pitch = Pitch > MaxMinCameraAngleX ? MaxMinCameraAngleX : Pitch;    
        Pitch = Pitch < -MaxMinCameraAngleX ? -MaxMinCameraAngleX : Pitch;    

        var cameraVector = new Vector3(0.0f, Yaw, 0.0f);

        if(Vehicle){
            //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0); 
            // Rotate player
            // var inverseRotation =  Quaternion.Inverse(Vehicle.transform.rotation);
            // inverseRotation = Quaternion.Euler(inverseRotation.eulerAngles.x,0,inverseRotation.eulerAngles.z);
            // VehicleRotationCorrector.transform.rotation = inverseRotation;
            transform.rotation = Quaternion.Euler(0, Yaw, 0);

            // Move player
            var inputVector = transform.rotation * new Vector3(Input.GetAxis("Horizontal")*OnVehicleWalkSpeed,0,Input.GetAxis("Vertical")*OnVehicleWalkSpeed);
            
            if(DistanceTooVehicle>.6f){
                CC.SimpleMove(inputVector);    
            }else{
                CC.Move(inputVector);
            }
        } else if(RB) {
            
            // Move player
            var inputVector = new Vector3(Input.GetAxis("Horizontal")*MoveSpeed,0,Input.GetAxis("Vertical")*MoveSpeed);
            RB.AddRelativeForce(inputVector,ForceMode.Force);
            // Rotate player
            RB.rotation = Quaternion.Euler(RB.rotation.eulerAngles.x, Yaw, RB.rotation.eulerAngles.z);
            
            

            //RB.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*RotateSpeed,0),ForceMode.Force);
        }
        // Move Camera
        Camera.transform.localRotation = Quaternion.Euler(Pitch,0,0);
    }
        
    // is called by Unity when ever a value in the inspector is changed
    private void OnValidate()
    {
        // Make sure editor updates property
        Vehicle = _Vehicle;
    }

    /// <summary>
    /// Enable walk mode for player
    /// </summary>
    /// <returns></returns>
    public Rigidbody LandWalkMode(){
        this.RB = GetComponent<Rigidbody>();
        if(!this.RB){
            this.RB = gameObject.AddComponent<Rigidbody>();
        }
        // Add any Rigidbody settings here 
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        CC.enabled = false;
        BC.enabled = true;
        return this.RB;
    }

    /// <summary>
    /// Enable vehicle walk mode for player
    /// </summary>
    public void VehicleWalkMode()
    {
        if(GetComponent<Rigidbody>()) {
            Destroy(gameObject.GetComponent<Rigidbody>());
        }
        transform.SetParent(Vehicle.transform, true);
        BC.enabled = false;
        CC.enabled = true;
    }

}
