using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : Entity {
    private List<Vector2Int> path;
    private int pathIndex;
    private float tmrLookForTarget;
    public Entity target;

    public float lookForTargetRate = 0.5f;
    public float stopChasingDistance = 10f;
    public float aggroDistance = 4f;

    private string[] targets = { "Player", "Clone" };

    public AudioClip attackSound;

    private void FixedUpdate() {
        if (isDead) {
            animator.SetBool("walking", false);
            return;
        }

        tmrLookForTarget += Time.deltaTime;
        if (tmrLookForTarget > lookForTargetRate) {
            if(target == null) {
                foreach (Entity e in FindObjectsOfType<Entity>()) {
                    if (CanTarget(e)) SetTarget(e);
                }
            } else {
                if (target.isDead || Vector2.Distance(rb.position, target.transform.position) > stopChasingDistance) {
                    target = null;
                } else {
                    if (!RaycastTarget(target) || pathIndex == path.Count - 1) SetTarget(target);
                }
            }
            tmrLookForTarget = 0;
        }

        if (target != null) {
            animator.SetBool("walking", true);
            rb.position = Vector2.MoveTowards(rb.position, nextPos, speed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, nextPos) <= minDistanceBeforeNextMove && pathIndex < path.Count - 1) {
                pathIndex++;
                SetNextPos(new Vector2(path[pathIndex].x + 0.5f, path[pathIndex].y + 0.5f));
            }
        } else {
            animator.SetBool("walking", false);
        }
    }

    private bool RaycastTarget(Entity e) {
        RaycastHit2D hit = Physics2D.Linecast(rb.position, e.transform.position);
        if (hit.collider == null) return false;
        return hit.collider.name == e.name;
    }

    private bool CanTarget(Entity e) {
        if (e.isDead) return false;
        if (!targets.Contains(e.tag)) return false;
        if (Vector2.Distance(rb.position, e.transform.position) > aggroDistance) return false;
        return RaycastTarget(e);
    }

    private void SetTarget(Entity e) {
        target = e;

        Vector2Int from = (Vector2Int)GetTilePosition();
        Vector2Int to = (Vector2Int)target.GetTilePosition();
        pathIndex = 0;
        path = new AStar(PathfindingMap.Instance.grid.ToArray()).Path(from, to);
        SetNextPos(new Vector2(path[pathIndex].x + 0.5f, path[pathIndex].y + 0.5f));
    }

    public override void OnReset() {
        base.OnReset();
        target = null;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (targets.Contains(collision.gameObject.tag)) {
            collision.gameObject.GetComponent<Entity>().OnDeath();
            PlaySound(attackSound, 0.3f);

            target = null;
        }
    }

    public override void OnDeath() {
        if (isDead) return;

        base.OnDeath();
        target = null;
    }
}
