using UnityEngine;

public class Envelope
{
    public float release = 0.2f;
    public float current = 0f;

    private float minTime = 0.003f;
    private float delta = 0f;


    public void Bang()
    {
        delta = 1f / (minTime * SynthConfig.kSampleRate);
    }


    public void Update()
    {
        if (delta > 0f) {
            current += delta;
            if (current >= 1f) {
                current = 1f;
                var r = Mathf.Max(release, minTime);
                delta = -1f / (r * SynthConfig.kSampleRate);
            }
        } else {
            current = Mathf.Max(current + delta, 0f);
        }
    }
}
