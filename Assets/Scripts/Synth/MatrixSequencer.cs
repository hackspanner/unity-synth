public class MatrixSequencer
{
    public bool[,] triggers;

    private int position = -1;
    private float delta = 0f;
    private float counter = 1f;
    

    public MatrixSequencer(int aBpm, int channels, int length)
    {
        triggers = new bool[channels, length];
        delta = 4f * aBpm / (SynthConfig.kSampleRate * 60f); 
    }
    

    public void SetTrack(int channel, bool[] track)
    {
        for (var i = 0; i < track.Length ; i++)
        {
            triggers[channel, i] = track[i];
        }
    }


    public void Reset()
    {
        position = -1;
        counter = 1f;
    }


    public bool Run()
    {
        bool bang = (counter >= 1f);
        
        if (bang)
        {
            if (++position == triggers.GetLength(1)) position = 0;
            counter -= 1f;
        }
        
        counter += delta;
        
        return bang;
    }

    
    public bool GetCurrent(int channel)
    {
        return triggers[channel, position];
    }
}
