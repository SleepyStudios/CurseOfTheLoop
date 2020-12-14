using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Entity : MonoBehaviour {
    protected Tilemap floor, walls;
    protected Vector2 nextPos, initialPos;

    [SerializeField]
    protected float speed = 5f;

    [SerializeField]
    protected float minDistanceBeforeNextMove = 0.1f;

    protected Rigidbody2D rb;
    protected Animator animator;

    public bool isDead;

    private AudioSource sfx;
    public AudioClip footstepSound;

    private float tmrFootsteps;
    public float footstepTime = 0.75f;

    protected void Awake() {
        floor = GameObject.Find("Floor").GetComponent<Tilemap>();
        walls = GameObject.Find("Walls").GetComponent<Tilemap>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();
    }

    protected void Start() {
        initialPos = transform.position;
        SetNextPos(initialPos);
    }

    private void LateUpdate() {
        if(animator.GetBool("walking") && footstepSound != null) {
            tmrFootsteps += Time.deltaTime;
            if (tmrFootsteps >= footstepTime) {
                PlaySound(footstepSound, 0.25f);
                tmrFootsteps = 0;
            }
        }
    }

    public bool IsOnTile(Vector2Int tile) {
        return Mathf.Approximately(transform.position.x, tile.x) && Mathf.Approximately(transform.position.y, tile.y);
    }

    public Vector3Int GetTilePosition() {
        return floor.WorldToCell(transform.position);
    }

    public virtual void OnDeath() {
        isDead = true;

        ParticleSystem ps = gameObject.GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (isDead) return;
        if (collision.CompareTag("PressurePlate")) {
            collision.GetComponent<PressurePlate>().OnTriggered(this);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (isDead) return;
        if (collision.CompareTag("PressurePlate")) {
            collision.GetComponent<PressurePlate>().OnUntriggered(this);
        }
    }

    protected bool CanMove(Vector2 dir) {
        if (isDead) return false;
        return PathfindingMap.Instance.CanMove(transform.position + (Vector3)dir);
    }

    public virtual void OnReset() {
        isDead = false;
        transform.position = initialPos;
        SetNextPos(initialPos);
    }

    protected void SetNextPos(Vector2 nextPos) {
        this.nextPos = nextPos;
        Vector3 normal = Vector3.Normalize((Vector3)nextPos - transform.position);
        if (!gameObject.CompareTag("Clone")) SetAngle(normal);
    }

    protected void SetAngle(Vector2 dir) {
        // brain stopped working around here
        float angle = 0;
        if (dir.x == -1) {
            angle = -180;
        } else if (dir.x == 1) {
            angle = 0;
        } else if (dir.y == -1) {
            angle = -90;
        } else if (dir.y == 1) {
            angle = 90;
        }

        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    protected void PlaySound(AudioClip clip, float pitchShift = 0f) {
        sfx.clip = clip;
        if (!Mathf.Approximately(pitchShift, 0f)) {
            sfx.pitch = Random.Range(1f - pitchShift, 1f + pitchShift);
        } else {
            sfx.pitch = 1f;
        }
        sfx.Play();
    }
}
