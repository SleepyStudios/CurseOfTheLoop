using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TreasureLight : MonoBehaviour {
    public new Light2D light;
    public float flickerRange = 0.4f, flickerUpdateTime = 0.05f;
    float startingItensity, tmrFlicker, targetIntensity;

    public float rotateSpeed;

    void Start() {
        startingItensity = light.intensity;
    }

    void Update() {
        if (light.enabled) {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, 0.05f);

            tmrFlicker += Time.deltaTime;
            if (tmrFlicker >= flickerUpdateTime) {
                targetIntensity = Random.Range(startingItensity - flickerRange, targetIntensity + flickerRange);
                tmrFlicker = 0;
            }
        }

        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
