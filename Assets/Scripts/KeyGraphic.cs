using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGraphic : MonoBehaviour {
    private Player p;

    void Start() {
        p = FindObjectOfType<Player>();
    }

    void Update() {
        RectTransform r = gameObject.transform as RectTransform;
        r.sizeDelta = new Vector2(p.keys * 64, r.sizeDelta.y);
    }
}
