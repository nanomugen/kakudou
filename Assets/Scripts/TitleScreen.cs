using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadSceneAsync("France");
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }
    void MoveCamera(){
        float mouseMove = Input.GetAxis("Mouse X");
        if(mouseMove>=0.2f || mouseMove<=-0.2f){
            Camera.main.transform.position+=Camera.main.transform.right*mouseMove*0.7f;
            Camera.main.transform.position = Camera.main.transform.position.normalized*15;
            Camera.main.transform.LookAt(Vector3.zero);
        }
        //Debug.Log("distance: "+ Vector3.Distance(Camera.main.transform.position,this.transform.position));
    }

    public void newGame(){
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("France"));
        SceneManager.LoadScene("France", LoadSceneMode.Single);
        //Application.LoadLevel("France");
    }
}
