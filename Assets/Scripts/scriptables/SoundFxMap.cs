using AYellowpaper.SerializedCollections;
using managers;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(fileName = "_SoundFxList", menuName = "SoundFxMap")]
    public class SoundFxMap : ScriptableObject {
        public SerializedDictionary<SoundManager.Sound, AudioClip> audioClips = new ();
    }
    
    // public static class SoundFxListExtension {
    //     public static AudioClip GetRandomClip(this SoundFxMap soundFxMap) {
    //         var index = Random.Range(0, soundFxMap.audioClips.Keys.Count);
    //         return soundFxMap.audioClips.Values.ToArray()[index];
    //     }
    // }
}