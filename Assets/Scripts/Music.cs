using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public static Music Instance { get; private set; }

    private void Awake() {
        DontDestroyOnLoad(this);

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
