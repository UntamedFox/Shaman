using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckpoint;

    private BasicPlatformerController player;

    public float respawnDelay;

	public float igniteDelay;

	private int columnCounter;

	private int totalColumns = 4;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<BasicPlatformerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (columnCounter == totalColumns) {
		//if (columnCounter == 1) {
			player.enabled = false;
			GameObject light = GameObject.Find ("light");
			SpriteRenderer sr = light.GetComponent<SpriteRenderer> ();
			sr.sortingOrder = 0;
			Debug.Log ("You win");
		}
	}

    public void RespawnPlayer() {
        StartCoroutine("RespawnPlayerCo");
    }

    public IEnumerator RespawnPlayerCo()
    {
        player.skeletonAnimation.state.SetAnimation(0, "death", false);
        player.enabled = false;

		GameObject scream = GameObject.Find ("scream");
		AudioSource screamAudio = scream.GetComponent<AudioSource> ();
		screamAudio.Play ();

        yield return new WaitForSeconds(respawnDelay);


        //Debug.Log("Player respawned");

		Application.LoadLevel (Application.loadedLevelName);
        //player.transform.position = currentCheckpoint.transform.position;
        //player.enabled = true;
        //player.skeletonAnimation.state.SetAnimation(0, "idle", false);
    }

	public void Touch() {
		StartCoroutine("TouchCo");
	}

	public IEnumerator TouchCo() {
		player.skeletonAnimation.state.SetAnimation(0, "atack", false);
		player.enabled = false;
		yield return new WaitForSeconds(igniteDelay);
		player.enabled = true;
		columnCounter++;

		GameObject soldelreloj = GameObject.Find("soldelreloj");
		SpriteRenderer spriteRenderer = soldelreloj.GetComponent<SpriteRenderer> ();

		float rgba = 100.0f / (totalColumns * 1.0f / columnCounter) / 100.0f;
		Debug.Log (rgba);
		spriteRenderer.color = new Color ( rgba, rgba, rgba);

		GameObject pilar = GameObject.Find ("pilar");
		AudioSource pilarAudio = pilar.GetComponent<AudioSource> ();
		pilarAudio.Play ();


		player.skeletonAnimation.state.SetAnimation(0, "idle", false);
	}
		
}

