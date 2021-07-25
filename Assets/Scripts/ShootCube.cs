using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCube : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject reset_place;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player"){
            Vector3 cameraoffset = reset_place.transform.position-other.gameObject.transform.position;
            //Camera.main.transform.position = Camera.main.transform.position+new Vector3(cameraoffset.x,0f,cameraoffset.z);
            Camera.main.transform.position = Camera.main.transform.position+cameraoffset;
            other.gameObject.transform.position = reset_place.transform.position;
        }
    }
}
