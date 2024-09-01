using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;

    public void TakeDamage(int damage) {
        health -= damage;
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
