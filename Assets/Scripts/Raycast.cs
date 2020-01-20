using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    public int damage = 1;
    private float FireRate;
    public float weaponRange = 50f;
    public float hitForce;
    private Camera fpsCam;
    private WaitForSeconds shotDuration;
    private AudioSource gunAudio;
    public ParticleSystem laserLine;
    private float nextFire;
    void Start () {
        gunAudio = GetComponent<AudioSource> ();
        fpsCam = GetComponentInParent<Camera> ();
        shotDuration = new WaitForSeconds (laserLine.main.duration);
        FireRate = laserLine.main.duration;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire) {
            nextFire = Time.deltaTime + FireRate;
            StartCoroutine (shotEffect ());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
                Shootable health = hit.collider.GetComponent<Shootable> ();
                laserLine.transform.LookAt (hit.point);
                if (health != null) {
                    health.Damage (damage);
                }
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce (-hit.normal * hitForce);
                }
            } else {
                laserLine.transform.LookAt (rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }
    private IEnumerator shotEffect () {
        gunAudio.Play ();
        laserLine.Play ();
        yield return shotDuration;
    }
}