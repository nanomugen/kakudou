using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatFall : MonoBehaviour{
    [SerializeField]
    private float timeFall=1f;
    private Vector3 originalPosition;
    private bool fallInUse;
    private Rigidbody rigidbody;
    void Start(){
        originalPosition = this.transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody>();

    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            if(!fallInUse){
                Debug.Log("getting down");
            fallInUse=true;
            StartCoroutine(getDown());
            }
                
        }
    }

    IEnumerator getDown(){
        yield return new WaitForSeconds(timeFall);
        Debug.Log("after count 5");
        rigidbody.useGravity=true;
        rigidbody.isKinematic=false;
        
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 6 && other.gameObject.transform.parent != this.transform){
            Debug.Log("relou no chão");
            transform.position = originalPosition;
            rigidbody.useGravity=false;
            rigidbody.isKinematic=true;
            fallInUse=false;
        }
    }
}
