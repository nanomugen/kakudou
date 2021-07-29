using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatMove : MonoBehaviour
{
    [SerializeField]
    private bool xMove;
    [SerializeField]
    private bool yMove;
    [SerializeField]
    private bool zMove;
    [SerializeField]
    private float xMax;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float yMax;
    [SerializeField]
    private float yMin;
    [SerializeField]
    private float zMax;
    [SerializeField]
    private float zMin;
    [SerializeField]
    private float velocity;

    private Vector3 originalPosition;
    private float xUp=1,yUp=1,zUp=1;
    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

    }
    void Move(){
        if(xMove){
            //Debug.Log(this.transform.position);
            if(this.transform.position.x > originalPosition.x+xMax||this.transform.position.x < originalPosition.x+xMin){
                
                xUp*=-1;
            }
            this.transform.position+=new Vector3(Time.deltaTime*xUp*velocity,0f,0f);
            //rigidbody.velocity = new Vector3(xUp,0f,0f);
            //rigidbody.AddForce(new Vector3(Time.deltaTime*xUp,0f,0f));
        }
        if(yMove){
            if(this.transform.position.y > originalPosition.y+yMax||this.transform.position.y < originalPosition.y+yMin){
                yUp*=-1;
            }
            this.transform.position+=new Vector3(0f,Time.deltaTime*yUp*velocity,0f);
        }
        if(zMove){
            if(this.transform.position.z > originalPosition.z+zMax||this.transform.position.z < originalPosition.z+zMin){
                zUp*=-1;
            }
            this.transform.position+=new Vector3(0f,0f,Time.deltaTime*zUp*velocity);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){    
            other.transform.parent = this.transform;
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag=="Player"){
            other.transform.parent = null;
        }
        
    }

}
