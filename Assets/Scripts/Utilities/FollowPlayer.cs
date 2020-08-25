using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform Player;
    private Animator animator;

    private float moveSpeed = 6.5f;
    public float minDist = 5;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<FinalBattleController>().polesHaveAppeared) //If poles have appeared, Dooley can start following
        {
            transform.LookAt(Player);

            if (Vector3.Distance(transform.position, Player.position) >= minDist)
            {
                animator.SetBool("isWalking", true);

                GetComponent<CharacterController>().SimpleMove(transform.forward * moveSpeed);
            }
            else animator.SetBool("isWalking", false);

        }
    }
}
