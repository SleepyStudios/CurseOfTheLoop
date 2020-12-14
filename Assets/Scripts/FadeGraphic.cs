using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeGraphic : MonoBehaviour {
    public static FadeGraphic Instance { get; private set; }
    private Animator animator;

    private void Awake() {
        Instance = this;
        animator = GetComponent<Animator>();
        animator.SetTrigger("fadeOut");
    }

    public void FadeBlack() {
        animator.SetTrigger("fadeBlack");
    }

    public void FadeWhite() {
        animator.SetTrigger("fadeWhite");
    }

    public void OnFadeInFinished() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
