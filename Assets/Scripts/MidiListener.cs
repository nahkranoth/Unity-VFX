using System;
using System.Collections;
using System.Collections.Generic;
using Minis;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidiListener : MonoBehaviour
{
   private void Start()
   {
      InputSystem.onDeviceChange += (device, change) =>
      {
         if (change != InputDeviceChange.Added) return;

         var midiDevice = device as Minis.MidiDevice;
         if (midiDevice == null) return;

         midiDevice.onWillNoteOn += (note, velocity) => {
            // Note that you can't use note.velocity because the state
            // hasn't been updated yet (as this is "will" event). The note
            // object is only useful to specify the target note (note
            // number, channel number, device name, etc.) Use the velocity
            // argument as an input note velocity.
            
            Debug.Log("Note ON");
            Debug.Log(note.noteNumber);
            Debug.Log(velocity);
            //
            // Debug.Log(string.Format(
            //    "Note On #{0} ({1}) vel:{2:0.00} ch:{3} dev:'{4}'",
            //    note.noteNumber,
            //    note.shortDisplayName,
            //    velocity,
            //    (note.device as Minis.MidiDevice)?.channel,
            //    note.device.description.product
            // ));
         };

         midiDevice.onWillNoteOff += (note) => {
            
            Debug.Log("Note OFF");
            Debug.Log(note.noteNumber);
          
         };
      };
      
      
   }

}
