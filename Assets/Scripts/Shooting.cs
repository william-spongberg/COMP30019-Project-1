using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooting : MonoBehaviour
{
    public Transform camera;
    public Transform firePoint;
    public GameObject projectile;

    public KeyCode shootKey = KeyCode.Mouse0;
    public float fireRate;
    public float projectileSpeed;

    bool canFire;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(shootKey) && canFire)
        {
            canFire = false;

            GameObject nail = Instantiate(projectile, firePoint.position, camera.rotation);

            Rigidbody nailrb = nail.GetComponent<Rigidbody>();

            Vector3 shotForce = camera.transform.forward * projectileSpeed;

            nailrb.AddForce(shotForce, ForceMode.Impulse);
        }

        Invoke(nameof(ResetFire), fireRate);
    }

    private void ResetFire()
    {
        canFire = true;
    }
}
