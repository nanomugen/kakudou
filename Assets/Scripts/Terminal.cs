using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour{
    [SerializeField]
    private int idmessage=0;
    private string[] message = new string[4];
    private Vector3 myPosition;
    private bool terminalClose;
    private GUIStyle myStyle = new GUIStyle();
    //private 
    // Start is called before the first frame update
    void Start(){
        //myStyle = GUI.skin.box;
        myStyle.fontSize = 14;
        myStyle.alignment = TextAnchor.MiddleCenter;
        //myStyle.wordWrap = true;
        message[0] = "This is Paris, and here you can see the EIffel Tower, \nat least what remains of it... but looks like you can put it back together, \nfind the hints and move the sliders inside the misterious rocks \nto reconnect it all, then you can take a good picture from the best point of view.";
        message[1] = "to begin with the static platforms challenge...\nthe answer will be the debut of tower...\nyou have two sliders, you should use two digits in each one...\nyou may need to split some number...";
        message[2] = "for the second challenge you will need to know the height of the tower...\nagain, you need 2 numbers... \n2 you will need to split 3 for 2...";
        message[3] = "did you know that the humans used this tower to television signal transmission?\n if you can say the year that it started...\nthen you can solve the last puzzle...";
    }

    // Update is called once per frame
    void Update(){
        
    }
    void OnGUI(){
        if(terminalClose){
            myPosition = Camera.main.WorldToScreenPoint(transform.position);
            //GUI.Box(new Rect(myPosition.x-256, Screen.height-myPosition.y, 512, 256),message[idmessage]);
            GUILayout.BeginArea(new Rect(myPosition.x-256, Screen.height-myPosition.y, 512, 512));
            GUILayout.Box(message[idmessage]);//,myStyle);
            GUILayout.EndArea();
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "Player"){
            //Debug.Log("entered here");
            terminalClose=true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            //Debug.Log("entered here");
            terminalClose=false;
        }
    }

}
