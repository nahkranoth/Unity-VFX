using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VEffectToAudioSpecBinder : MonoBehaviour
{
    public VisualEffect vEffect;
    public AudioSpectrumAnalyser audioSpec;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vEffect.SetFloat("AudioBandOne", audioSpec.GetBandValue(1));
        vEffect.SetFloat("AudioBandTwo", audioSpec.GetBandValue(3));
        vEffect.SetFloat("AudioBandThree", audioSpec.GetBandValue(5));
        vEffect.SetFloat("AudioBandFour", audioSpec.GetBandValue(6));
    }
}
