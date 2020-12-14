using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour {

    public virtual void Trigger(Entity target, AudioSource triggerSound) {
        triggerSound.Play();
    }

    public virtual void Untrigger(Entity target) { }

    public virtual void OnReset() { }
}
