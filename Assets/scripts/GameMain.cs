using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameMain : MonoBehaviour {

    public GameObject bird;
    public GameObject readyPic;
    public GameObject tipPic;
    public GameObject scoreMgr;
    public GameObject pipeSpawner;

    private bool gameStarted = false;


    public SerialController serialController;
    string message;
    int intMessage;
    int caseSwitch;
    public float spawnTime;

    // Use this for initialization
    void Start () {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        
    }

    // Update is called once per frame
    void Update () {
        if (!gameStarted && Input.GetButtonDown("Fire1"))
        {
            gameStarted = true;
           
            
            StartGame();
          
        }
    }

    private void StartGame()
    {
        BirdControl control = bird.GetComponent<BirdControl>();
        control.inGame = true;
        control.JumpUp();

        readyPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
        tipPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);

        scoreMgr.GetComponent<ScoreMgr>().SetScore(0);
        pipeSpawner.GetComponent<PipeSpawner>().StartSpawning();
    }

    public void setSpawnTime(float _spwanTime)
    {
        spawnTime = _spwanTime;
    }


}
