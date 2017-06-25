public class Amplifier
{
    public float level = 1f;

    private Envelope env;
    

    public Amplifier(Envelope anEnv)
    {
        env = anEnv;
    }
    

    public float Run(float input)
    {
        return input * level * env.current;
    }
}
