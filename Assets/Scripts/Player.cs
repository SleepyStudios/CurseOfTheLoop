using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Entity {
    [SerializeField]
    private GameObject clonePrefab;

    private List<Action> actions = new List<Action>();
    private float actionTmr;

    public int keys;

    public float deathDelayTime = 1f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate() {
        actionTmr += Time.fixedDeltaTime;

        if (!isDead) rb.position = Vector2.MoveTowards(rb.position, nextPos, speed * Time.fixedDeltaTime);
        if (Vector2.Distance(rb.position, nextPos) <= minDistanceBeforeNextMove) {
            if (!Mathf.Approximately(Input.GetAxisRaw("Horizontal"), 0)) {
                Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                if (CanMove(dir)) Move(dir);
            } else if (!Mathf.Approximately(Input.GetAxisRaw("Vertical"), 0)) {
                Vector2 dir = new Vector2(0, Input.GetAxisRaw("Vertical"));
                if (CanMove(dir)) Move(dir);
            } else {
                animator.SetBool("walking", false);
            }
        }

        if (isDead) {
            animator.SetBool("walking", false);
        }
    }

    private void Move(Vector2 dir) {
        if (isDead) return;

        SetNextPos(nextPos + dir);
        SetAngle(dir);
        animator.SetBool("walking", true);

        actions.Add(new Action() {
            nextPos = nextPos,
            delay = actionTmr,
            type = "MOVE",
            angle = transform.eulerAngles.z
        });
        actionTmr = 0;
    }

    public override void OnDeath() {
        if (isDead) return;

        base.OnDeath();

        keys = 0;

        Invoke("FadeOut", deathDelayTime - 0.4f);

        Invoke("DelayedOnDeath", deathDelayTime);
    }

    private void FadeOut() {
        FadeGraphic.Instance.FadeBlack();
    }

    private void DelayedOnDeath() {
        Clone c = Instantiate(clonePrefab, Vector2.zero, Quaternion.identity).GetComponent<Clone>();

        actions.Add(new Action() {
            nextPos = nextPos,
            delay = actionTmr,
            type = "DIE",
            angle = transform.eulerAngles.z
        });
        c.SetActions(new List<Action>(actions), initialPos, speed);

        actions.Clear();
        actionTmr = 0;

        ResetAll();
    }

    public void PickupKey() {
        keys++;
        GameObject.Find("KeyGraphic").GetComponent<Image>().enabled = true;
    }

    public void ResetAll() {
        foreach (Entity e in FindObjectsOfType<Entity>()) {
            e.OnReset();
        }

        foreach (PressurePlateTrigger t in FindObjectsOfType<PressurePlateTrigger>()) {
            t.OnReset();
        }

        PathfindingMap.Instance.Recalculate();
    }
}
