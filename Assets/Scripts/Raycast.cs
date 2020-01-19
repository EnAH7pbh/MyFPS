using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    public int damage = 1;
    public float FireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100;
    public Transform gunEnd;
    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds (0.7f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;
    void Start () {
        laserLine = GetComponent<LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        fpsCam = GetComponentInParent<Camera> ();
    }

    void Update () {
        if (Input.GetButtonDown ("Fire1") && Time.time > nextFire) {
            nextFire = Time.deltaTime + FireRate;
            StartCoroutine (shotEffect ());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
            RaycastHit hit;
            laserLine.SetPosition (0, gunEnd.position);
            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
                laserLine.SetPosition (1, hit.point);
                Shootable health = hit.collider.GetComponent<Shootable> ();
                if (health != null) {
                    health.Damage (damage);
                }
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce (-hit.normal * hitForce);
                }
            } else {
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }
    private IEnumerator shotEffect () {
        gunAudio.Play ();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}