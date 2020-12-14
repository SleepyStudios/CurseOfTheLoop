using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Clone : Entity {
    private List<Action> actions;
    private bool canMove;

    public int keys;

    private int invokeRound;

    public GameObject keyPrefab;

    public void SetActions(List<Action> actions, Vector2 initialPos, float speed) {
        this.actions = actions;
        this.initialPos = transform.position = initialPos;
        this.speed = speed;

        InvokeActions();
    }

    private void InvokeActions() {
        invokeRound++;
        float cumulativeDelay = 0;

        foreach (Action a in actions) {
            cumulativeDelay += a.delay;
            StartCoroutine(InvokeAction(invokeRound, a, cumulativeDelay));
        }
    }

    IEnumerator InvokeAction(int invokeRound, Action a, float delay) {
        if (isDead || invokeRound != this.invokeRound) yield break;
        yield return new WaitForSeconds(delay);
        if (isDead || invokeRound != this.invokeRound) yield break;

        switch (a.type) {
            case "MOVE":
                SetNextPos(a.nextPos);
                transform.eulerAngles = new Vector3(0, 0, a.angle);
                animator.SetBool("walking", true);
                canMove = true;
                break;
            case "DIE":
                SetNextPos(a.nextPos);
                transform.eulerAngles = new Vector3(0, 0, a.angle);
                animator.SetBool("walking", false);
                canMove = false;
                break;
        }

        yield return null;
    }

    private void FixedUpdate() {
        if (isDead) return;

        if (canMove) {
            rb.position = Vector2.MoveTowards(rb.position, nextPos, speed * Time.fixedDeltaTime);

            if (rb.position == nextPos) {
                animator.SetBool("walking", false);
            }
        }
    }

    public override void OnDeath() {
        if (isDead) return;

        base.OnDeath();

        StopAllCoroutines();

        animator.SetBool("walking", false);

        GetComponent<BoxCollider2D>().enabled = false;

        for (int i = 0; i < keys; i++) {
            GameObject k = Instantiate(keyPrefab, transform.position, Quaternion.identity);
            k.GetComponentInChildren<Key>().SetCreated();
        }
        keys = 0;
    }

    public override void OnReset() {
        base.OnReset();

        GetComponent<BoxCollider2D>().enabled = true;

        InvokeActions();
    }

    public void PickupKey() {
        keys++;
    }
}
