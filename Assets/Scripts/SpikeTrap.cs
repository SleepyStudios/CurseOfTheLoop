using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : PressurePlateTrigger {
    private Animator animator;
    private float tmrRetract;

    [SerializeField]
    private float retractTime = 5f;

    private bool isSafe;

    private void Start() {
        animator = GetComponent<Animator>();
        transform.Translate(new Vector3(0, 0.125f, 0));
    }

    public override void Trigger(Entity target, AudioSource triggerSound) {
        if (target.isDead || isSafe) return;

        base.Trigger(target, triggerSound);

        if (!animator.GetBool("triggered")) tmrRetract = 0;
        animator.SetBool("triggered", true);

        if (target.CompareTag("Player")) CameraShake.Instance.Shake(3f, 0.3f);

        target.OnDeath();
    }

    private void Update() {
        if (animator.GetBool("triggered")) {
            tmrRetract += Time.deltaTime;
            if (tmrRetract >= retractTime) {
                animator.SetBool("triggered", false);
                tmrRetract = 0;
            }
        }
    }

    public override void OnReset() {
        base.OnReset();

        tmrRetract = 0;
        animator.SetBool("triggered", false);
    }

    public void ToggleSafety() {
        isSafe = !isSafe;
    }
}
