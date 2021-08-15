using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZouRock : MonoBehaviour
{   
    
    public int number;
    private bool moveTower;
    [SerializeField]
    private Vector3 offsetButton;

    private Texture button_e;
    private Vector3 my_position;
    //public static bool RockInteract;

    // Start is called before the first frame update
    
    public int getNumber(){
        return this.number;
    }
    void OnGUI() {
        if(moveTower && !BotController.RockInteract){
            my_position = Camera.main.WorldToScreenPoint(transform.position)+ offsetButton;
            Rect ButtonSpace = new Rect(my_position.x, Screen.height-my_position.y, 64, 64);
            GUI.Box(ButtonSpace,button_e);
        }
    }

    void Start()
    {
        button_e = Resources.Load<Texture2D>("Etc/key");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            moveTower=true;
            //Debug.Log("entered here");
            
        }
            
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag =="Player")
            moveTower=false;
    }




}
