using UnityEngine;
using System.Collections;

public class PipeSpawner : MonoBehaviour {

    float spawnTime=1.72f;		// The amount of time between each spawn.
	public float spawnDelay = 3f;		// The amount of time before spawning starts.
	public GameObject pipe;	
	public float[] heights;


	public SerialController serialController;
	string message;
	int intMessage;
	int caseSwitch;


	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
	}



    public void StartSpawning()
    {
        

        message = serialController.ReadSerialMessage();




        if (message == null)
            return;
        
        intMessage = int.Parse(message);

        //// Check if the message is plain data or a connect/disconnect event.
        //if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        //    Debug.Log("Connection established");
        //else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        //    Debug.Log("Connection attempt failed or disconnection detected");


        Debug.Log("Taha");
        if (intMessage >= 4000000 && intMessage < 5000000)
        {
            caseSwitch = intMessage - 4000000;

            switch (caseSwitch)
            {

                case 0:
                    spawnTime = 0.5f;
                    Debug.Log("Poteniometer: 0.5f");
                    break;
                case 1:
                    spawnTime = 1.0f;
                    Debug.Log("Poteniometer: 1.0f");
                    break;
                case 2:
                    spawnTime = 1.5f;
                    Debug.Log("Poteniometer: 1.5f");
                    break;
                case 3:
                   spawnTime = 2.0f;
                    Debug.Log("Poteniometer: 2.0f");
                    break;
                case 4:
                    spawnTime = 2.5f;
                    Debug.Log("Poteniometer: 2.5f");
                    break;
                case 5:
                   spawnTime = 3.0f;
                    Debug.Log("Poteniometer: 3.0f");
                    break;
                case 6:
                    spawnTime = 3.5f;
                    Debug.Log("Poteniometer: 3.5f");
                    break;
                case 7:
                    spawnTime = 4.0f;
                    Debug.Log("Poteniometer: 4.0f");
                    break;
                case 8:
                    spawnTime = 4.5f;
                    Debug.Log("Poteniometer: 4.0f");
                    break;
                case 9:
                    spawnTime = 5.0f;
                    Debug.Log("Poteniometer: 4.0f");
                    break;
                case 10:
                    spawnTime = 5.5f;
                    Debug.Log("Poteniometer: 4.0f");
                    break;
                default:
                    spawnTime = 1.72f;
                    Debug.Log("Poteniometer: 1.72");
                    break;
            }
        }

        InvokeRepeating("Spawn", spawnDelay, spawnTime);
    }


    void Spawn ()
	{
		int heightIndex = Random.Range(0, heights.Length);
		Vector2 pos = new Vector2(transform.position.x, heights[heightIndex]);

		Instantiate(pipe, pos, transform.rotation);
	}

	public void GameOver()
	{
		CancelInvoke("Spawn");
	}

    
}
