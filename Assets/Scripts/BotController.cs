using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class BotController : MonoBehaviour{
    [SerializeField]
    private float Speed=5F;
    [SerializeField]
    private float JumpForce=7F;
    [SerializeField]
    private float CameraHeight=1f;
    [SerializeField]
    private float CameraDistance=3.5f;
    [SerializeField]
    private float CameraMoveMin=0.15f;
    [SerializeField]
    private float Sensibility=10f;
    [SerializeField]
    private float SensibilityFPS=2f;
    [SerializeField]
    private GameObject misteryPrefab;


    private Rigidbody rigidbody;
    private bool ground;
    private bool isJumping;
    private Animator animator;

    private bool MenuOpened;
    private bool MessageSpawn;
    private float MessageCount;
    private float MessageTimeout=15f;
    private string Message;
    private GUIStyle guiStyle;

    private Texture2D lastPic;

    private float AngleX=0f;
    private float AngleY=0f;
    private float XLimit;
    private Vector3 CameraPosition3P;
    private Vector3 CameraPosition1P;
    private Vector3 Center1P;
    private Vector3 pauseVelocity;
    private float Zoom;
    private bool FirstPerson;
    private bool onMovingPlat;

    private bool RockInteractPermit;
    public static bool RockInteract;
    private int InteractNumber;
    private bool observer;

    //eiffel
    private GameObject eiffel_p1;
    private int p1x,p1y,p1z;
    private GameObject eiffel_p2;
    private int p2x,p2y,p2z;
    private GameObject eiffel_p3;
    private int p3x,p3y,p3z;
    private bool p1,p2,p3;

    private GameObject goal;


    //sound
    private AudioClip walkSound;
    private AudioClip jumpSound;
    private AudioClip landSound;
    private AudioClip misterySound;
    private AudioClip photoSound;
    private AudioSource audioScr;

    //particle effect
    [SerializeField]
    private GameObject dust;
    private bool win;
    private bool winEnd;
    void OnGUI(){
        
        if(MenuOpened){
            GUILayout.BeginArea(new Rect(10, 10, 200, 300));
                GUILayout.Label("CameraHeight: "+CameraHeight);
                GUILayout.Label("CameraDistance: "+CameraDistance);
                GUILayout.Label("Mouse Sensibility: "+Sensibility);
                Sensibility = GUILayout.HorizontalSlider(Sensibility,5f,50f);
                GUILayout.Label("Mouse Sensibility FPS: "+SensibilityFPS);
                SensibilityFPS = GUILayout.HorizontalSlider(SensibilityFPS,0.5f,8f);
            GUILayout.EndArea();
            
            GUILayout.BeginArea(new Rect(10,Screen.height-30,50,20));
                if(GUILayout.Button("Quit")){
                    Application.Quit();
                }
            GUILayout.EndArea();
            //rigidbody.velocity = Vector3.zero;
        }
        if(MenuOpened){//colocar uma variavel aqui pra mostrar quando tirar a foto tambem
            GUILayout.BeginArea(new Rect(10,Screen.height-200,160,90));
                if(lastPic!=null){
                    GUILayout.Box(lastPic);
                }
            GUILayout.EndArea();
        }
        if(win){
            GUILayout.BeginArea(new Rect(10, 10, 200, 300));
                GUILayout.Label("Written and Produced by Elton");
                GUILayout.Label("Musics\nCirque Français\nHot and tasty\nLa Fleur\nParis Pigalle\nPetite Chanson\nVoila Paris");
                GUILayout.Label("Thank you for playing!");
            GUILayout.EndArea();
        }

        if(winEnd){
            
            GUILayout.BeginArea(new Rect(10,Screen.height-30,50,20));
                if(GUILayout.Button("Reset")){
                    SceneManager.LoadScene("TitleScene");
                }
            GUILayout.EndArea();
        }

        if(MessageSpawn){
            if(MessageCount<=MessageTimeout){
                //GUILayout.BeginArea(new Rect((Screen.width/2)-240,Screen.height-80,480,40));
                GUI.backgroundColor = Color.black;
                GUI.Box(new Rect((Screen.width/2)-240,Screen.height-80,480,40),Message,guiStyle);
                //GUILayout.EndArea();
                MessageCount+=Time.deltaTime;
            }
            else{
                MessageSpawn=false;
            }
            
        }

        if(RockInteract && !MenuOpened){
            if(Input.GetKeyDown(KeyCode.E)){
                RockInteract=false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if(InteractNumber==1 && !p1){
                p1x = Mathf.RoundToInt(eiffel_p1.transform.position.x);
                //p1y = Mathf.RoundToInt(eiffel_p1.transform.position.y);
                p1z = Mathf.RoundToInt(eiffel_p1.transform.position.z);
                GUILayout.BeginArea(new Rect((Screen.width/2)-250,10,500,400));
                    GUILayout.Label("X Axis: "+(p1x+83));
                    p1x = (int)GUILayout.HorizontalSlider(p1x,-250f,250f);
                    GUILayout.Label("Z Axis: "+(p1z+9));
                    p1z = (int)GUILayout.HorizontalSlider(p1z,-250f,250f);
                    eiffel_p1.transform.position = new Vector3(p1x,eiffel_p1.transform.position.y,p1z);

                GUILayout.EndArea();
                if(eiffel_p1.transform.localPosition.x==0f && eiffel_p1.transform.localPosition.z==0f){
                    //MAKE EFFECT AND SOUND
                    solveMistery();
                    p1=true;
                }
                //Debug.Log(eiffel_p1.transform.localPosition);
            }
            if(InteractNumber==2 && !p2){
                p2x = Mathf.RoundToInt(eiffel_p2.transform.position.x);
                p2z = Mathf.RoundToInt(eiffel_p2.transform.position.z);
                GUILayout.BeginArea(new Rect((Screen.width/2)-250,10,500,400));
                    GUILayout.Label("X Axis: "+(p2x+68));
                    p2x = (int)GUILayout.HorizontalSlider(p2x,-250f,250f);
                    GUILayout.Label("Z Axis: "+(p2z-56));
                    p2z = (int)GUILayout.HorizontalSlider(p2z,-250f,250f);
                    eiffel_p2.transform.position = new Vector3(p2x,eiffel_p2.transform.position.y,p2z);

                GUILayout.EndArea();
                Debug.Log(eiffel_p2.transform.localPosition);
                if(eiffel_p2.transform.localPosition.x==0f && eiffel_p2.transform.localPosition.z==0f){
                    //MAKE EFFECT AND SOUND
                    solveMistery();
                    p2=true;
                }
            }
            if(InteractNumber==3 && !p3){
                p3x = Mathf.RoundToInt(eiffel_p3.transform.position.x);
                p3z = Mathf.RoundToInt(eiffel_p3.transform.position.z);
                GUILayout.BeginArea(new Rect((Screen.width/2)-250,10,500,400));
                    GUILayout.Label("X Axis: "+(p3x+84));
                    p3x = (int)GUILayout.HorizontalSlider(p3x,-250f,250f);
                    GUILayout.Label("Z Axis: "+(p3z-23));
                    p3z = (int)GUILayout.HorizontalSlider(p3z,-250f,250f);
                    eiffel_p3.transform.position = new Vector3(p3x,eiffel_p3.transform.position.y,p3z);

                GUILayout.EndArea();
                if(eiffel_p3.transform.localPosition.x==0f && eiffel_p3.transform.localPosition.z==0f){
                    //MAKE EFFECT AND SOUND
                    solveMistery();
                    p3=true;
                }
                Debug.Log(eiffel_p3.transform.localPosition);
            }
        }
        
    }
    void MessageStart(string msg){
        Message = msg;
        MessageCount=0f;
        MessageSpawn=true;
    }
    void Start(){
        Vector3 cameraDir = new Vector3(1f,0f,-1f).normalized;
        CameraHeight = 1f;
        CameraPosition3P= transform.position+cameraDir*CameraDistance+new Vector3(0f,CameraHeight,0f);
        Camera.main.transform.position = CameraPosition3P;
        transform.LookAt(new Vector3(Camera.main.transform.position.x,transform.position.y,Camera.main.transform.position.z));
        rigidbody = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        goal = GameObject.Find("Goal");
        eiffel_p1 = GameObject.Find("eiffel_p1");
        eiffel_p2 = GameObject.Find("eiffel_p2");
        eiffel_p3 = GameObject.Find("eiffel_p3");

        Cursor.lockState = CursorLockMode.Locked;
        audioScr = gameObject.GetComponent<AudioSource>();
        walkSound = Resources.Load<AudioClip>("Sounds/botwalk");
        landSound = Resources.Load<AudioClip>("Sounds/land");
        jumpSound = Resources.Load<AudioClip>("Sounds/jump_custo");
        misterySound = Resources.Load<AudioClip>("Sounds/fanfare");
        photoSound = Resources.Load<AudioClip>("Sounds/pictobox");
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 32;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleCenter;
        //lastPic = Resources.Load<Texture>("screenshot.png");
        Directory.CreateDirectory("Pictures");
        lastPic = new Texture2D(Screen.width,Screen.height,TextureFormat.ARGB32,false);
        

        

    }
    void Update(){
        if(!win){
            Move();
            Jump();
            Menu();
            CameraMove();
            Freeze();
            Snap();
            RockMove();
        }
        else{
            Win();
        }
        
        
    }
    void Snap(){
        if(FirstPerson && Input.GetButtonDown("LeftClick") && !MenuOpened){
            Debug.Log("take snap "+ System.DateTime.Now.ToString("yyyyMMddHHmmssffff"));
            lastPic=ScreenCapture.CaptureScreenshotAsTexture(1);
            string nameofpicture = "Pictures"+Path.DirectorySeparatorChar+"screenshot"+System.DateTime.Now.ToString("yyyyMMddHHmmssffff")+".png";
            //PlayerPrefs.SetString("PicName",nameofpicture);
            File.WriteAllBytes(nameofpicture,lastPic.EncodeToPNG());
            MessageStart("You Took a Picture!");
            if(audioScr.isPlaying){
                audioScr.Stop();
            }
            audioScr.clip = photoSound;
            audioScr.Play();
            Ray photoRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0f));
            if(Physics.Raycast(photoRay,out RaycastHit hit, Mathf.Infinity)){
                if(hit.transform.name == "Goal"||hit.transform.name =="eiffel_p1"||hit.transform.name =="eiffel_p2"||hit.transform.name =="eiffel_p3"){
                    if(observer && p1 && p2 && p3){
                        //Debug.Log("you win");
                        //SceneManager.LoadScene("WinScene");
                        win=true;
                        
                        if(audioScr.isPlaying){
                            audioScr.Stop();
                        }
                        audioScr.clip = misterySound;
                        audioScr.Play();
                        //Win();

                    }
                    else{
                        //Debug.Log("1 "+observer+p1+p2+p3);
                    }
                }
                else{
                    //Debug.Log("2");
                }
                    
            }
            else{
                //Debug.Log("3");
            }
            
        }
    }
    void Win(){
        if(Camera.main.fieldOfView>=0.5f){
            Camera.main.fieldOfView -= Time.deltaTime;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            winEnd=true;
        }
    }

    void CameraMove(){
        if(!MenuOpened && !RockInteract && !isJumping && !onMovingPlat){
            if((Input.GetAxisRaw("LT")>=0.8f||Input.GetKey(KeyCode.Tab)) && !FirstPerson){
                FirstPerson=true;
                CameraPosition1P = transform.position+new Vector3(0f,1.5f,0f)+0.75f*transform.forward;
                Center1P = CameraPosition1P;
                Zoom=0f;
                rigidbody.velocity=Vector3.zero;
                Camera.main.transform.position = CameraPosition1P;
                Camera.main.transform.forward=transform.forward;
                //Debug.Log("Camera position: "+ Camera.main.transform.position+" Camera.localRotation: "+Camera.main.transform.localRotation.eulerAngles);
                XLimit = Camera.main.transform.localRotation.eulerAngles.y;
                AngleY=XLimit;
                AngleX=0f;
                animator.SetBool("WALK",false);
                animator.SetBool("JUMP",false);
                if(audioScr.isPlaying){// && audioScr.clip==walkSound){
                    audioScr.Stop();
                    audioScr.loop=false;
                }
            }
            else{
                if((Input.GetAxisRaw("LT")<=0.3f && !Input.GetKey(KeyCode.Tab)) && FirstPerson){
                    FirstPerson=false;
                    Camera.main.transform.position = CameraPosition3P; 
                }
            }
            if(FirstPerson){
                float sideMovement = Input.GetAxis("Horizontal");
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                Zoom = Mathf.Clamp(Zoom+scroll,0f,2f);
                CameraPosition1P += 0.1f*(transform.right*sideMovement);
                if(Vector3.Distance(CameraPosition1P,Center1P)>2f){//DEFINIR RAIO DE DISTANCIA MÁXIMA#####
                    CameraPosition1P =Center1P + 2*((CameraPosition1P-Center1P).normalized);
                }
                Camera.main.transform.position = CameraPosition1P+Zoom*transform.forward;
                AngleX += -Input.GetAxis("Mouse Y")*SensibilityFPS;
                AngleY += Input.GetAxis("Mouse X")*SensibilityFPS;
                AngleX = Mathf.Clamp(AngleX,-50f,50f);
                AngleY = Mathf.Clamp(AngleY,XLimit-50f,XLimit+50f);
                Camera.main.transform.localRotation = Quaternion.Euler(AngleX,AngleY,0f);
                //Debug.Log("Camera position: "+ Camera.main.transform.position+" Camera.localRotation: "+Camera.main.transform.localRotation.eulerAngles);
            }
        }
    }
    void Move(){
        if(!FirstPerson && !MenuOpened && !RockInteract){
            float sideMovement = Input.GetAxisRaw("Horizontal");
            float frontMovement = Input.GetAxisRaw("Vertical");
            Vector3 movementVector = new Vector3(sideMovement,0f,frontMovement);
            
            //front -> vector that point to the front of player based on the camera
            Vector3 front = (transform.position-new Vector3(Camera.main.transform.position.x,transform.position.y,Camera.main.transform.position.z)).normalized;
            //side -> vector that point to the right(?) direction of the body, ortogonal to the front and up vector
            Vector3 side = Vector3.Cross(Vector3.up,front).normalized;
            Vector3 SideVelocity = side*sideMovement;//*Time.deltaTime;
            Vector3 FrontVelocity = front*frontMovement;//*Time.deltaTime;
            transform.LookAt(transform.position + (SideVelocity+FrontVelocity));
            Vector3 velocity = Speed*((SideVelocity+FrontVelocity).normalized);
            rigidbody.velocity = new Vector3(velocity.x,rigidbody.velocity.y,velocity.z);
            if(movementVector.magnitude >0.3f){    
                animator.SetBool("WALK",true);
                if(!audioScr.isPlaying&&ground){
                    audioScr.clip = walkSound;
                    audioScr.loop=true;
                    audioScr.Play();
                }
            }
            else{
                if(rigidbody.velocity.y >0 && ground && !isJumping){
                    rigidbody.velocity = new Vector3(0f,0f,0f);
                }
                animator.SetBool("WALK",false);
                if(audioScr.isPlaying && audioScr.clip==walkSound){
                    audioScr.Stop();
                    audioScr.loop=false;
                }
            }
            float sideMouse = Input.GetAxisRaw("Mouse X");
            float upMouse = Input.GetAxisRaw("Mouse Y");
            //Debug.Log(Input.GetAxisRaw("Mouse ScrollWheel"));
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if(sideMouse > CameraMoveMin || sideMouse < -CameraMoveMin){
                Vector3 sideCamera = Vector3.Cross(transform.position - Camera.main.transform.position,Vector3.up).normalized;
                CameraPosition3P += sideCamera*sideMouse*Sensibility*Time.deltaTime;//*(-1);
                Camera.main.transform.position = CameraPosition3P;
                
            }
            if((upMouse < -CameraMoveMin && CameraHeight <=5f) || (upMouse > CameraMoveMin && CameraHeight >=0.5f)){
                CameraHeight= Mathf.Clamp(CameraHeight - upMouse*Time.deltaTime*Sensibility,0.5f,5f);
            }
            if((scroll>=0.1f && CameraDistance >=3f)||(scroll<=-0.1 && CameraDistance <=10f)){
                CameraDistance-=scroll;
            }
            
            front = (transform.position-new Vector3(Camera.main.transform.position.x,transform.position.y,Camera.main.transform.position.z)).normalized;
            side = Vector3.Cross(Vector3.up,front).normalized;
            CameraPosition3P = transform.position+CameraDistance*(-1)*front+new Vector3(0f,CameraHeight,0f);//NESSA FUNÇÃO ACOMPANHA SEMPRE CENTRALIZADO, QUANDO PULA A CAMERA MEXE
            Camera.main.transform.position = CameraPosition3P;
            //Camera.main.transform.position = new Vector3(transform.position.x,0f,transform.position.z)+CameraDistance*(-1)*front+new Vector3(0f,CameraHeight,0f);//NESSA FUNÇÃO A CAMERA ESTÁ SEMPRE NA MESMA ALTURA, O PULO FICA MELHOR, A CAMERA NAO MEXE AO PULAR
            
            Camera.main.transform.LookAt(transform.position);
        }
    }
    void Jump(){
        if(Input.GetButtonDown("B") && ground && !MenuOpened && !FirstPerson && !RockInteract){
            isJumping=true;
            rigidbody.velocity += Vector3.up*JumpForce;
            if(audioScr.isPlaying){
                audioScr.Stop();
            }
            audioScr.clip=jumpSound;
            audioScr.loop=false;
            audioScr.Play();
            Instantiate(dust,transform.position+new Vector3(0f,0.1f,0f),Quaternion.identity);
        }
    }
    void Menu(){

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(MenuOpened){
                if(!RockInteract){
                    Cursor.lockState = CursorLockMode.Locked;
                    
                }
                rigidbody.velocity = pauseVelocity;
                rigidbody.isKinematic = false;
                MenuOpened = false;
            }
            else{
                Cursor.lockState = CursorLockMode.None;
                MenuOpened = true;
                animator.SetBool("WALK",false);
                animator.SetBool("JUMP",false);
                pauseVelocity = rigidbody.velocity;
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
                if(audioScr.isPlaying){// && audioScr.clip==walkSound){
                    audioScr.Stop();
                    audioScr.loop=false;
                }
            }
            //Debug.Log("MenuOpened: "+ MenuOpened + " Cursor.lockState: "+ Cursor.lockState);
        }
    }
    void Freeze(){

    }
    private void solveMistery(){
        GameObject fireworks = Instantiate<GameObject>(misteryPrefab,transform.position+Camera.main.transform.forward*2f,misteryPrefab.transform.rotation,this.transform);
        //fireworks.transform.position = this.transform.forward;
        if(audioScr.isPlaying){
            audioScr.Stop();
        }
        audioScr.clip = misterySound;
        audioScr.Play();
        
    }
     void RockMove(){
        if(RockInteract){
            if((p1 && InteractNumber==1)||(p2 && InteractNumber==2)||(p3 && InteractNumber ==3)){
                return;
            }
            Camera.main.transform.LookAt(goal.transform.position);
            
        }
    }
    private void OnCollisionEnter(Collision other) {
        //6. floor
        if(other.gameObject.layer == 6){
            isJumping=false;//SÓ POR CAUSA DA RAMPA
            ground =true;
            animator.SetBool("JUMP",false);
            if(audioScr.isPlaying){
                audioScr.Stop();
            }
            audioScr.loop=false;
            audioScr.clip = landSound;
            audioScr.Play();
            Instantiate(dust,transform.position+new Vector3(0f,0.1f,0f),Quaternion.identity);
        }
    }
    private void OnCollisionStay(Collision other) {
        //6. floor
        if(other.gameObject.layer == 6){
            //Debug.Log("floor collision enter");
            ground =true;
            animator.SetBool("JUMP",false);
            ContactPoint contact = other.contacts[0];
            //Debug.Log("Player: "+ transform.position+ " Collision: "+contact.point);//fazer comparação com ambos para definir quando pusou de fato
        }
        if(other.gameObject.tag == "observer"){
            observer=true;
        }
    }
    private void OnCollisionExit(Collision other) {
        //6. floor
        if(other.gameObject.layer == 6){
            this.transform.parent=null;
            onMovingPlat=false;
            ground =false;
            animator.SetBool("JUMP",true);
            if(audioScr.isPlaying && audioScr.clip==walkSound){
                audioScr.Stop();
                audioScr.loop=false;
            }
        }
        if(other.gameObject.tag == "observer"){
            observer=false;
        }
    }
    private void OnTriggerStay(Collider other) {
        if(other.tag == "ZouRock"){
            if(Input.GetKeyDown(KeyCode.E) && !RockInteractPermit && !RockInteract && !MenuOpened){
                Debug.Log("RockInteract true");
                RockInteractPermit=true;
            }
            if(Input.GetKeyUp(KeyCode.E) && RockInteractPermit && !MenuOpened){//cuidado para nao sair do ontrigger stay com o "e apertado" pode travar
                InteractNumber = other.GetComponent<ZouRock>().getNumber();
                RockInteractPermit=false;
                if((p1 && InteractNumber==1)||(p2 && InteractNumber==2)||(p3 && InteractNumber ==3)){
                
                }
                else{
                    RockInteract=true;
                    Cursor.lockState = CursorLockMode.None;
                }
                
                
                
            }
        }
        if(other.tag == "MovingPlat"){
            onMovingPlat=true;
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "MovingPlat"){
            onMovingPlat=false;
        }
        
    }


}
