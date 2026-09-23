using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float updateRate = 2.0f; // Updates 2 times per second

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    void Update()
    {
        frameCount++;
        // Use unscaledDeltaTime so the counter remains accurate even if the game is paused/slowed down
        dt += Time.unscaledDeltaTime; 

        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            fpsText.text = $"FPS: {Mathf.RoundToInt(fps)}";
            
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }
}