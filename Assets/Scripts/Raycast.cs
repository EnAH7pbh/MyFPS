using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour {
    public int damage = 1;
    private float FireRate;
    public float weaponRange = 50f;
    public float hitForce;
    private Camera fpsCam;
    private WaitForSeconds shotDuration;
    private AudioSource gunAudio;
    public ParticleSystem laserLine;
    public ParticleSystem laserEffect;
    public Slider laserEnergy;
    private float nextFire;
    void Start () {
        gunAudio = GetComponent<AudioSource> ();
        fpsCam = GetComponentInParent<Camera> ();
        shotDuration = new WaitForSeconds (4f);
        FireRate = 4f;
    }

    void Update () {
        if (Input.GetMouseButtonDown (0) && nextFire > FireRate) {
            nextFire = 0;
            StartCoroutine (shotEffect ());
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
                Shootable health = hit.collider.GetComponent<Shootable> ();
                laserLine.transform.LookAt (hit.point);
                var clone = Instantiate (laserEffect, hit.point, Quaternion.LookRotation (hit.normal));
                Destroy (clone.gameObject, 4);
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
        nextFire += Time.deltaTime;
        laserEnergy.value = nextFire;
    }
    private IEnumerator shotEffect () {
        gunAudio.Play ();
        laserLine.Play ();
        yield return shotDuration;
    }
}