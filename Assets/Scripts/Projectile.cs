using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private Rigidbody rb;

    private bool targetHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {

        if (targetHit){
            return;
        }
            
        else{
            targetHit = true;
        }
        if(collision.gameObject.GetComponent<EnemyHP>() != null)
        {
            EnemyHP enemy = collision.gameObject.GetComponent<EnemyHP>();

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
        rb.isKinematic = true;

        transform.SetParent(collision.transform);
    }
}
