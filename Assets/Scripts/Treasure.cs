using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Treasure : MonoBehaviour {
    public int numberOfLevels = 1;

    private void Start() {
        PlayerPrefs.SetInt("curLevel", SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            FadeGraphic.Instance.FadeWhite();
        }
    }

    public void ChangeScene(int index) {
        SceneManager.LoadSceneAsync(index);
    }
}
