using UnityEngine;

public class PauseGame : MonoBehaviour {
    public void display() {
        Debug.Log("Display Exit Game panel");
        gameObject.SetActive(true);
    }

    public void hide() {
        Debug.Log("Hide Exit Game panel");
        gameObject.SetActive(false);
    }
}
