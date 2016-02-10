using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

    public float moveSpeed;
    public bool moveRight;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool hittingWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        hittingWall = Physics.CheckSphere(wallCheck.position, wallCheckRadius, whatIsWall);

        if (hittingWall) {
            moveRight = !moveRight;
        }

        if (moveRight)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<Rigidbody>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody>().velocity.y);
        }
        else {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            GetComponent<Rigidbody>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody>().velocity.y);
        }
	
	}
}
