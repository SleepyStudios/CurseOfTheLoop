using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Key : PressurePlateTrigger {
    private bool pickedUp, created;

    public override void Trigger(Entity target, AudioSource triggerSound) {
        if (pickedUp) return;
        if (target.isDead) return;

        if (target.CompareTag("Player")) {
            target.GetComponent<Player>().PickupKey();
        } else if (target.CompareTag("Clone")) {
            target.GetComponent<Clone>().PickupKey();
        } else {
            return;
        }


        base.Trigger(target, triggerSound);

        pickedUp = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<ShadowCaster2D>().enabled = false;
    }

    public override void OnReset() {
        base.OnReset();

        if (created) {
            Destroy(transform.parent.gameObject);
        } else {
            pickedUp = false;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<ShadowCaster2D>().enabled = true;
        }
    }

    public void SetCreated() {
        created = true;
    }
}
