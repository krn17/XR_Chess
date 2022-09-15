using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Scriptable Objects/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public SoundGroup[] soundGroups;

    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

	// initializes the sounds dictionary
    public void Initialize()
    {
		for (int i = 0; i < soundGroups.Length; i++)
        {
			SoundGroup soundGroup = soundGroups[i];
			groupDictionary.Add(soundGroup.GroupID, soundGroup.group);
        }
    }

    public AudioClip GetClipFromName(string name)
    {
        if(groupDictionary.ContainsKey(name))
        {
            AudioClip[] sounds = groupDictionary[name];
            return sounds[Random.Range(0,sounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string GroupID;
        public AudioClip[] group;
    }
}
