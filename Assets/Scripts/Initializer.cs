using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

class Initializer : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;

        //    SynthConfig.kSampleRate = AudioSettings.outputSampleRate;
        AudioSettings.outputSampleRate = SynthConfig.kSampleRate;

        yield return null;

        SceneManager.LoadScene("Scenes/Init");
    }
}
