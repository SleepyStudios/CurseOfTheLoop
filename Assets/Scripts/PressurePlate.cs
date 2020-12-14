using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlate : MonoBehaviour {
    public PressurePlateTrigger controlledObject;

    private AudioSource triggerSound;
    public bool triggered;

    private void Start() {
        triggerSound = GetComponent<AudioSource>();
    }

    public void OnTriggered(Entity e) {
        triggered = true;
        controlledObject.Trigger(e, triggerSound);
    }

    public void OnUntriggered(Entity e) {
        triggered = false;
        controlledObject.Untrigger(e);
    }
}
