using UnityEngine;

class Oscillator
{
	private float x = 0f;
	private float step = 0f;
	

	public void SetNote(int note)
    {
		float freq = 440f * Mathf.Pow(2f, 1f * (note - 69) / 12f);
		step = freq / SynthConfig.kSampleRate;
	}
	

	public float Run()
    {
		x += step;
		if (x > 1f) x -= 1f;
		return x * 2f - 1f;
	}
}
