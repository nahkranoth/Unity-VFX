using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class PostPToAudioSpecBinder : MonoBehaviour
{
    public Volume PostPVolume;
    public AudioSpectrumAnalyser audioSpec;
    // Update is called once per frame
    private ChromaticAberration cAbberation;
    private Bloom cBloom;
    private void Start()
    {
        ChromaticAberration tmp;
        if( PostPVolume.profile.TryGet<ChromaticAberration>( out tmp ) )
        {
            cAbberation = tmp;
            cAbberation.intensity.overrideState = true;
        }
        
        Bloom tmpB;
        if( PostPVolume.profile.TryGet<Bloom>( out tmpB ) )
        {
            cBloom = tmpB;
            cBloom.intensity.overrideState = true;
        }
    }

    void Update()
    {
        if (cAbberation)
        {
            cAbberation.intensity.value = audioSpec.GetBandValue(0) / 2;
        }

        if (cBloom)
        {
            cBloom.intensity.value = audioSpec.GetBandValue(0) * 4;
        }
    }
}
