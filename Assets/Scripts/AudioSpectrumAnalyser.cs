using UnityEngine;
using UnityEngine.VFX;


[RequireComponent(typeof(AudioSource))]
public class AudioSpectrumAnalyser : MonoBehaviour
{
    public AudioSource audio;
    [SerializeField] private float lerpTime = 0.1f;

    float[] samples = new float[512];
    public float[] freqBands = new float[8];
    
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFullSpectrum();
        UpdateFrequencyBands();
    }

    public float GetBandValue(int id)
    {
        if (id >= freqBands.Length) Debug.LogError($"Outside of freq band range. Only {freqBands.Length}");
        return freqBands[id];
    }
    
    private void UpdateFullSpectrum()
    {
        audio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
    
    private void UpdateFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float avarage = 0f;
            int sampleCount = (int) Mathf.Pow(2, i) * 2;

            if (i == 7) sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                avarage += samples[count] * (count + 1);
                count++;
            }

            avarage /= count;

            freqBands[i] = Mathf.Lerp(freqBands[i], avarage * 10, lerpTime);
        }
    }
}
