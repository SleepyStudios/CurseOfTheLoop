using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

enum PatrolDirection {
    Up,
    Down,
    Left,
    Right
}

public class Mummy : Entity {
    [SerializeField]
    private PatrolDirection patrolDirection;

    private bool isEating;
    private float tmrEating;

    public float eatingTime = 3f;

    private Vector3 oldEuler;

    private string[] targets = { "Player", "Clone" };

    public AudioClip attackSound, deathSound;

    private void FixedUpdate() {
        if (isDead) {
            animator.SetBool("walking", false);
            return;
        }

        Vector3Int dir = Vector3Int.zero;

        switch (patrolDirection) {
            case PatrolDirection.Up:
                dir = new Vector3Int(0, 1, 0);
                if (!CanMove((Vector2Int)dir)) {
                    patrolDirection = PatrolDirection.Down;
                    return;
                }
                break;
            case PatrolDirection.Down:
                dir = new Vector3Int(0, -1, 0);
                if (!CanMove((Vector2Int)dir)) {
                    patrolDirection = PatrolDirection.Up;
                    return;
                }
                break;
            case PatrolDirection.Left:
                dir = new Vector3Int(1, 0, 0);
                if (!CanMove((Vector2Int)dir)) {
                    patrolDirection = PatrolDirection.Right;
                    return;
                }
                break;
            case PatrolDirection.Right:
                dir = new Vector3Int(-1, 0, 0);
                if (!CanMove((Vector2Int)dir)) {
                    patrolDirection = PatrolDirection.Left;
                    return;
                }
                break;

        }

        if (!isEating) {
            rb.position = Vector2.MoveTowards(rb.position, nextPos, speed * Time.fixedDeltaTime);
            if (Vector2.Distance(rb.position, nextPos) <= minDistanceBeforeNextMove) {
                SetNextPos((Vector3)rb.position + dir);
                animator.SetBool("walking", true);
            }
        } else {
            animator.SetBool("walking", false);

            tmrEating += Time.deltaTime;
            if (tmrEating >= eatingTime) {
                setEating(false);
                transform.eulerAngles = oldEuler;

                tmrEating = 0;
            }
        }
    }

    public override void OnReset() {
        base.OnReset();

        setEating(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isDead) return;

        if (!isEating && targets.Contains(collision.gameObject.tag)) {
            setEating(true);
            oldEuler = transform.eulerAngles;
            collision.gameObject.GetComponent<Entity>().OnDeath();

            Vector3 dir = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            PlaySound(attackSound, 0.3f);
            if (collision.gameObject.CompareTag("Player")) CameraShake.Instance.Shake(3f, 0.3f);
        } else if (collision.gameObject.CompareTag("Boulder")) {
            OnDeath();
        }
    }

    private void setEating(bool isEating) {
        this.isEating = isEating;
        tmrEating = 0;
        GetComponent<BoxCollider2D>().enabled = !isEating;
    }

    public override void OnDeath() {
        if (isDead) return;

        base.OnDeath();

        PlaySound(deathSound, 0.3f);
    }
}
