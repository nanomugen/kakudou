using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{


    //private string[] playList = {"m0","m1","m2","m3","m4","m5"};
    private int[] playList = {0,1,2,3,4,5};
    private int current=0;
    private AudioSource audioScr;
    //private AudioClip currentAudio;
    private AudioClip[] musics;
    //private ResourceRequest nextAudio;
    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
        printList();
        audioScr = gameObject.GetComponent<AudioSource>();
        //currentAudio = Resources.Load<AudioClip>("Music/"+playList[current++]);
        musics = Resources.LoadAll<AudioClip>("Music");
        //audioScr.clip = currentAudio;
        audioScr.clip = musics[playList[current++]];
        audioScr.Play();
        //loadNext();
        //playNext();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(Random.Range(0,playlistOrder.Length));
        if(!audioScr.isPlaying){
            Debug.Log("notplaying");
            playNext();
        }
        //Debug.Log(audioScr.time+"/"+audioScr.clip.length);
    }

    void Shuffle(){
        int n  = playList.Length;
        while (n > 1){
            int k = Random.Range(0,n--);
            //string temp = playList[n];
            int temp = playList[n];
            playList[n] = playList[k];
            playList[k] = temp;
        }

    }
    void printList(){
        string allS="";
        for(int i=0;i<playList.Length;i++){
            allS  += " "+playList[i];
        }
        Debug.Log(allS);
    }
    void playNext(){
        //currentAudio = nextAudio.asset as AudioClip;
        //audioScr.clip = currentAudio;
        audioScr.clip = musics[playList[(current++)%playList.Length]];
        audioScr.Play();
        //loadNext();
        Debug.Log("plaiyng!");
    }
    void loadNext(){
        //nextAudio = Resources.LoadAsync("Music/"+playList[(current)%playList.Length]);
        //current++;
    }
}
