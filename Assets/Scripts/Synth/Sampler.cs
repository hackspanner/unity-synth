using UnityEngine;


public class Sampler
{
    private float[] samples = {0f};
    private int position = 0;
    public float volume = 1f;
    

    public Sampler(AudioClip clip)
    {
        samples = new float[clip.samples];
        clip.GetData(samples, 0);
        position = samples.Length;
    }
    

    public void Bang()
    {
        position = 0;
    }
    

    public float Run()
    {
        if (position < samples.Length)
        {
            return samples[position++] * volume;
        }
        else
        {
            return 0f;
        }
    }
}
