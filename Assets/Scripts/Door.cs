using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PressurePlateTrigger {
    private Animator animator;

    public float explosionDelay = 0.5f;
    public AudioClip explosionSound;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public override void Trigger(Entity target, AudioSource triggerSound) {
        if (animator.GetBool("triggered")) return;

        if (target.CompareTag("Player")) {
            if (target.GetComponent<Player>().keys > 0) {
                target.GetComponent<Player>().keys--;
            } else {
                return;
            }
        } else if (target.CompareTag("Clone")) {
            if (target.GetComponent<Clone>().keys > 0) {
                target.GetComponent<Clone>().keys--;
            } else {
                return;
            }
        } else {
            return;
        }

        base.Trigger(target, triggerSound);

        Invoke("ExplodeDoor", explosionDelay);
    }

    private void ExplodeDoor() {
        transform.parent.gameObject.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        CameraShake.Instance.Shake(4f, 1f);

        animator.SetBool("triggered", true);
        PathfindingMap.Instance.Recalculate();

        gameObject.GetComponentInChildren<ParticleSystem>().Play();
    }

    public override void OnReset() {
        base.OnReset();

        animator.SetBool("triggered", false);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void DisableCollisionBox() {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnDoorAnimationFinished() {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool IsClosed() {
        return !animator.GetBool("triggered");
    }
}
