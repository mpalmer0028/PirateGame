using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoatScript : MonoBehaviour
{
    
    public float GasPower = 20;
    public float TurnPower = 75;
    public GameObject CollidersFBX;

    private Rigidbody RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Debug.Log(Input.GetAxis("Vertical"));
        RB.AddRelativeForce(new Vector3(0,0,Input.GetAxis("Vertical")*GasPower),ForceMode.Force);
        RB.AddRelativeTorque(new Vector3(0,Input.GetAxis("Horizontal")*TurnPower,0),ForceMode.Force);
    }

    public void RegisterColliders()
    {
        foreach(var collider in GetComponents<MeshCollider>()){
            DestroyImmediate(collider);
        }
        
        Debug.Log(CollidersFBX.transform.name);
        foreach(Transform child in CollidersFBX.transform){
            Debug.Log(child.name);
            var mc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
            mc.convex = true;
            mc.sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
            // Collapse colliders
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(mc, false);
        }
        // for(var i = 0; i <CollidersFBX.Length; i++){
        //     Debug.Log(CollidersFBX[i].name);
        // }
    }
}

[CustomEditor(typeof(BoatScript))]
public class BoatScriptEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var bs = (BoatScript)target;
        if (GUILayout.Button("Register New Colliders"))
        {
            bs.RegisterColliders();
        }
    }
}