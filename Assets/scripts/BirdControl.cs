using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BirdControl : MonoBehaviour {
    public GameObject gameMain;
    public SerialController serialController;
	int distance;
    string message;
    int intMessage;
    bool jump;
    float spawnTime;
    int caseSwitch;


    public int rotateRate = 10;
	public float upSpeed = 10;
    public GameObject scoreMgr;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;

    public bool inGame = false;

	private bool dead = false;
	private bool landed = false;

    private Sequence birdSequence;

    // Use this for initialization
    void Start () {
		serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

         



        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();

        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
            .SetLoops(-1);
    }
	
	// Update is called once per frame
	void Update () {
       
        if (!inGame)
        {
            return;
        }
        birdSequence.Kill();

        getSensorsInputs();

        if (!dead)
		{
            

            if (jump)
			{
                JumpUp();
			}
		}

		if (!landed)
		{
			float v = transform.GetComponent<Rigidbody2D>().velocity.y;
			
			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);
			
			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().rotation = -90;
		}



    }

    void getSensorsInputs()
    {

        message = serialController.ReadSerialMessage();


        jump = false;

        if (message == null)
            return;

        intMessage = int.Parse(message);

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");

            if (intMessage > 1000005 && intMessage < 1000025)
            {
                if (distance == 0)
                {
                    distance = intMessage;
                }

                //Debug.Log("Distance: " + intMessage);
                if (distance > intMessage)
                {
                    jump = true;
                    Debug.Log("Jump" + "distance: " + distance + " intMessage: " + intMessage);
                }
                distance = intMessage;
            return;
            }

            if (intMessage > 2000000 && intMessage < 3000000)
            {
            Debug.Log("Dead");
           
            }

      






    }

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
		{
            if (!dead)
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                foreach (GameObject g in objs)
                {
                    g.BroadcastMessage("GameOver");
                }

                GetComponent<Animator>().SetTrigger("die");
                AudioSource.PlayClipAtPoint(hit, Vector3.zero);
            }

			

			if (other.name == "land")
			{
				transform.GetComponent<Rigidbody2D>().gravityScale = 0;
				transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

				landed = true;
			}
		}

        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }


	}

    public void JumpUp()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }
	
	public void GameOver()
	{
        dead = true;
        serialController.SendSerialMessage("A");
        Application.LoadLevel(1);

    }
}
