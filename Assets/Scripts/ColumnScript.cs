using UnityEngine;
using System.Collections;

public class ColumnScript : MonoBehaviour {

	public LevelManager levelManager;

	public SpriteRenderer spriteRenderer;

	public Sprite newSprite;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.name);
		if (other.name == "Player") {
			//Debug.Log (spriteRenderer.sprite);
			levelManager.Touch();
			spriteRenderer.sprite = newSprite;
		}
	}
}
