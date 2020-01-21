using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour {
    public float speed;
    public int EnemyHealth = 30;
    public static int EnemyDamage = 1;
    public static bool GiveDamage = false;
    GameObject target;
    private Rigidbody rbd;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    // Start is called before the first frame update
    void Start () {
        agent = GetComponent<NavMeshAgent> ();
        target = GameObject.FindGameObjectWithTag ("Player");
        agent.speed = speed;
        agent.updateRotation = true;
        agent.updatePosition = true;
        rbd = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update () {
        if (target != null) {
            agent.SetDestination (target.transform.position);
        }
    }
}