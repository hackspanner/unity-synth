using UnityEngine;

[RequireComponent (typeof(AudioSource))]
class SynthController : MonoBehaviour
{
    public int bpm = 124;
    public GUISkin skin;
    public AudioClip[] drumClips;

    private bool isRunning = false;

    private Oscillator osc;
    private Envelope env;
    private LowPassFilter lpf;
    private Amplifier amp;
    private Sequencer seq;

    private Sampler[] drums;
    private MatrixSequencer drumSeq;


void Start() {
    osc = new Oscillator();
    env =  new Envelope();
    lpf = new LowPassFilter(env);
    amp = new Amplifier(env);
    
    lpf.cutoff = 0.2f;
    lpf.envMod = 0.2f;

    seq = new Sequencer(bpm,
        new int[]
        {
            30, 30, 42, 30,
            30, 29, 30, 29,
            30, 30, 42, 30,
            30, 42, 29, 30
        },
        new bool[]
        {
            true, true, true, true,
            true, true, false, true,
            true, false, false, true,
            true, true, false, true
        }
    );
    
    drums = new Sampler[drumClips.Length];
    for (var i = 0; i < drumClips.Length; i++) {
        drums[i] = new Sampler(drumClips[i]);
    }
    
    drumSeq = new MatrixSequencer(bpm, drumClips.Length, 16);
    drumSeq.SetTrack(0,
        new bool[]
        {
            true, false, false, false,
            true, false, false, false,
            true, false, false, false,
            true, false, false, false
        }
    );
    
    GetComponent<AudioSource>().clip = AudioClip.Create("(null)", 0xfffffff, 1, SynthConfig.kSampleRate, true, delegate(float[] data) {} );
    GetComponent<AudioSource>().Play();
}

void OnGUI() {
    var sw = Screen.width;
    var sh = Screen.height;
    
    GUI.skin = skin;
    
    GUILayout.BeginArea(new Rect(0.05f * sw, 0.05f * sh, 0.9f * sw, 0.9f * sh));
    GUILayout.BeginVertical();

    GUILayout.BeginHorizontal();
    GUILayout.Label("Note/Trig");
    for (var i = 0; i < 16; i++) {
        GUILayout.BeginVertical();
        var note = seq.notes[i];
        var trigger = seq.triggers[i];
        var noteInput = GUILayout.TextField(note.ToString(), 3);
        seq.triggers[i] = GUILayout.Toggle(trigger, "");
        if (System.Int32.TryParse(noteInput, out note))
        {
            seq.notes[i] = note;
        }
        GUILayout.EndVertical();
    }
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Label("Drums");
    GUILayout.BeginVertical();
    for (var row = drumSeq.triggers.GetLength(0) - 1; row >= 0 ; row--)
    {
        GUILayout.BeginHorizontal();
        for (var col = 0; col < drumSeq.triggers.GetLength(1); col++)
        {
            drumSeq.triggers[row, col] = GUILayout.Toggle(drumSeq.triggers[row, col], "");
        }
        GUILayout.EndHorizontal();
    }
    GUILayout.EndVertical();
    GUILayout.EndHorizontal();
    
    GUILayout.BeginHorizontal();
    GUILayout.Label("Cutoff", GUILayout.Width(0.2f * sw));
    lpf.cutoff = GUILayout.HorizontalSlider(lpf.cutoff, 0f, 1f);
    GUILayout.EndHorizontal();
    
    GUILayout.BeginHorizontal();
    GUILayout.Label("Resonance", GUILayout.Width(0.2f * sw));
    lpf.resonance = GUILayout.HorizontalSlider(lpf.resonance, 0f, 1f);
    GUILayout.EndHorizontal();

    amp.level = 1f + lpf.resonance * 1.6f;

    GUILayout.BeginHorizontal();
    GUILayout.Label("EnvMod", GUILayout.Width(0.2f * sw));
    lpf.envMod = GUILayout.HorizontalSlider(lpf.envMod, 0f, 1f);
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    GUILayout.Label("Decay", GUILayout.Width(0.2f * sw));
    env.release = GUILayout.HorizontalSlider(env.release, 0f, 0.4f); 
    GUILayout.EndHorizontal();

    GUILayout.BeginHorizontal();
    if (GUILayout.Button("PLAY")) {
        seq.Reset();
        drumSeq.Reset();
        isRunning = true;
    }
    
    if (GUILayout.Button("STOP")) {
        isRunning = false;
    }
    GUILayout.EndHorizontal();
    
    GUILayout.EndVertical();
    GUILayout.EndArea();
}

void OnAudioFilterRead(float[] data, int channels) {
    // Asserts channels == 2
    for (var i = 0; i < data.Length; i += 2) {
        if (isRunning && seq.Run()) {
            osc.SetNote(seq.currentNote);
            if (seq.currentTrigger) {
                env.Bang();
            }
        }
        if (isRunning && drumSeq.Run()) {
            for (var tr = 0; tr < drumSeq.triggers.GetLength(0); tr++) {
                if (drumSeq.GetCurrent(tr)) drums[tr].Bang();
            }
        }

        var x = amp.Run(lpf.Run(osc.Run()));
        foreach (var sampler in drums) x += sampler.Run();
        data[i] = data[i + 1] = x;
        env.Update();
    }
}
}
