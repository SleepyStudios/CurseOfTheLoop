using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoulderDirection {
    Up,
    Down,
    Left,
    Right
}

public class BoulderTrap : PressurePlateTrigger {
    private bool triggered;

    [SerializeField]
    private GameObject boulderSpawn, boulderPrefab;

    [SerializeField]
    private BoulderDirection direction;

    public override void Trigger(Entity target, AudioSource triggerSound) {
        if (triggered) return;

        base.Trigger(target, triggerSound);

        GameObject b = Instantiate(boulderPrefab, boulderSpawn.transform.position, Quaternion.identity);
        b.GetComponent<Boulder>().Init(direction);

        triggered = true;
    }

    public override void OnReset() {
        base.OnReset();
        triggered = false;
    }
}
