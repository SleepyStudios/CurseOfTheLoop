using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Flicker : MonoBehaviour {
    public new Light2D light;
    public float flickerRange = 3f, flickerUpdateTime = 0.1f;
    float startingOuterRadius, tmrFlicker, targetOuterRadius;

    void Start() {
        startingOuterRadius = light.pointLightOuterRadius;
    }

    void Update() {
        if(light.enabled) {
            light.pointLightOuterRadius = Mathf.Lerp(light.pointLightOuterRadius, targetOuterRadius, 0.05f);

            tmrFlicker += Time.deltaTime;
            if (tmrFlicker >= flickerUpdateTime) {
                targetOuterRadius = Random.Range(startingOuterRadius - flickerRange, targetOuterRadius + flickerRange);
                tmrFlicker = 0;
            }
        }
    }
}
