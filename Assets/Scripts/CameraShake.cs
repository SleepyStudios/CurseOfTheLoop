using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float startIntensity, tmrShake, totalTime;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Shake(float intensity, float time) {
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = startIntensity = intensity;
        tmrShake = totalTime = time;
    }

    void Update() {
        if (tmrShake > 0) {
            tmrShake -= Time.deltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (tmrShake / totalTime));
        }
    }
}
