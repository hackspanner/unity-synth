// Moog-type LPF
// http://www.musicdsp.org/showArchiveComment.php?ArchiveID=26

using UnityEngine;

public class LowPassFilter
{
    public float cutoff = 1f;
    public float resonance = 0f;
    public float envMod = 0f;
    
    private Envelope env;
    
    private float i1 = 0f;
    private float i2 = 0f;
    private float i3 = 0f;
    private float i4 = 0f;
    
    private float o1 = 0f;
    private float o2 = 0f;
    private float o3 = 0f;
    private float o4 = 0f;
    

    public LowPassFilter(Envelope anEnv) {
        env = anEnv;
    }


    private float Saturate(float x)
    {
        return Mathf.Clamp(x, -1f, 1f);
    }

    
    public float Run(float input) {
        float f = Mathf.Clamp01(cutoff + env.current * envMod) * 1.16f;
        float fb = Mathf.Clamp01(resonance) * 4f * (1f - 0.15f * f * f);
        float x = input - o4 * fb;
        x = Mathf.Clamp(0.35013f * x * (f * f) * (f * f), -1f, 1f);
        o1 = Saturate(x + 0.3f * i1 + (1f - f) * o1);
        i1 = x;
        o2 = Saturate(o1 + 0.3f * i2 + (1f - f) * o2);
        i2 = o1;
        o3 = Saturate(o2 + 0.3f * i3 + (1f - f) * o3);
        i3 = o2;
        o4 = Saturate(o3 + 0.3f * i4 + (1f - f) * o4);
        i4 = o3;
        return o4;
    }
}
