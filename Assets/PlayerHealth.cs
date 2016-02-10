using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public float health = 100f;
    public float resetAfterDeathTime = 5f;

    private bool playerDead;

    void Awake()
    {

    }

    void PlayerDying()
    {
        playerDead = true;
    }

    void PlayerDead()
    {

    }
}
