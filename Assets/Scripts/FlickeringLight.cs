using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerControl : MonoBehaviour {
    public bool isFlickering = false;
    public float timeDelay;

    public GameObject lightObject;

    void Update()
{
    if (isFlickering == false)
    {
        StartCoroutine(FlickeringLight());
    }
}
            IEnumerator FlickeringLight()
            {
                isFlickering = true;
                lightObject.GetComponent<Light>().enabled = false;
                timeDelay = Random.Range(0.01f, 0.5f);
                yield return new WaitForSeconds(timeDelay);
                lightObject.GetComponent<Light>().enabled = true;
                timeDelay = Random.Range(0.01f, 0.5f);
                yield return new WaitForSeconds(timeDelay);
                isFlickering = false;
            }
        }
    