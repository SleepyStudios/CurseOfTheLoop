using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HiddenDoor : PressurePlateTrigger {
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Trigger(Entity target, AudioSource triggerSound) {
        base.Trigger(target, triggerSound);

        if (gameObject.GetComponentsInChildren<PressurePlate>().Count((c) => c.triggered) == transform.childCount) {
            animator.SetBool("open", true);
            PathfindingMap.Instance.Recalculate();
        }
    }

    public override void OnReset() {
        base.OnReset();

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        animator.SetBool("open", false);

        foreach (PressurePlate p in gameObject.GetComponentsInChildren<PressurePlate>()) {
            p.triggered = false;
        }
    }

    public void OnAnimFinished() {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool IsClosed() {
        return !animator.GetBool("open");
    }
}
