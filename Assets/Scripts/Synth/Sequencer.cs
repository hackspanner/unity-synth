public class Sequencer
{
    public int[] notes;
    public bool[] triggers;
    
    public int currentNote = -1;
    public bool currentTrigger = false;
    
    private int position = -1;
    
    private float delta = 0f;
    private float counter = 1f;

    
    public Sequencer(int aBpm, int[] initNotes, bool[] initTriggers)
    {
        notes = initNotes;
        triggers = initTriggers;
        delta = 4f * aBpm / (SynthConfig.kSampleRate * 60f); 
    }


    public void Reset()
    {
        position = -1;
        counter = 1f;
        currentNote = -1;
        currentTrigger = false;
    }

    public bool Run()
    {
        bool bang = (counter >= 1.0);
        
        if (bang) {
            if (++position == notes.Length) position = 0;
            currentNote = notes[position];
            currentTrigger = triggers[position];
            counter -= 1f;
        }
        
        counter += delta;
        
        return bang;
    }
}
