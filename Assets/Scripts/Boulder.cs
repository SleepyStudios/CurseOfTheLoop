using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : Entity {
    public BoulderDirection moveDir;
    private bool isMoving;

    [SerializeField]
    private GameObject particlesPrefab;

    public AudioClip hitClip;

    protected new void Start() {
        base.Start();
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        CameraShake.Instance.Shake(6f, 0.5f);
    }

    public void Init(BoulderDirection moveDir) {
        this.moveDir = moveDir;
        isMoving = true;
    }

    private void FixedUpdate() {
        if (!isMoving) {
            animator.SetBool("walking", false);
            return;
        }

        Vector3Int dir = Vector3Int.zero;

        switch (moveDir) {
            case BoulderDirection.Up:
                dir = new Vector3Int(0, 1, 0);
                break;
            case BoulderDirection.Down:
                dir = new Vector3Int(0, -1, 0);
                break;
            case BoulderDirection.Left:
                dir = new Vector3Int(1, 0, 0);
                break;
            case BoulderDirection.Right:
                dir = new Vector3Int(-1, 0, 0);
                break;
        }

        animator.SetBool("walking", true);

        rb.position = Vector2.MoveTowards(rb.position, nextPos, speed * Time.fixedDeltaTime);
        if (Vector2.Distance(rb.position, nextPos) <= minDistanceBeforeNextMove) {
            if (CanMove((Vector2Int)dir)) {
                PathfindingMap.Instance.Recalculate();
                SetNextPos((Vector3)rb.position + dir);
            } else {
                isMoving = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isMoving && collision.gameObject.GetComponent<Entity>() != null) {
            collision.gameObject.GetComponent<Entity>().OnDeath();
            PlaySound(hitClip, 0.1f);
            if (collision.gameObject.CompareTag("Player")) CameraShake.Instance.Shake(6f, 0.5f);
        }
    }

    public override void OnReset() {
        base.OnReset();

        Destroy(gameObject);
    }
}
