using UnityEngine;
using System.Collections;

namespace ChessAR
{
	public class AudioManager : MonoBehaviour
	{

		//Enum used for setting the master, sfx and music volume in a function
		public enum AudioChannel { Sfx, Music };

		public float sfxVolumePercent { get; private set; }
		public float musicVolumePercent { get; private set; }

		AudioSource sfx2DSource;                //Audio source for playing 2D sound effects
		AudioSource[] musicSources;             //Audio sources for backgroud music
		int activeMusicSourceIndex;             //The index of active music we are currently playing

		public SoundLibrary library;
		public static AudioManager instance;    //The singletone instance of AudioManager

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
				// initialize sound library
				library.Initialize();

				musicSources = new AudioSource[2];
				for (int i = 0; i < 2; i++)
				{
					GameObject newMusicSource = new GameObject("Music source " + (1 + i));
					musicSources[i] = newMusicSource.AddComponent<AudioSource>();
					musicSources[i].loop = true;
					newMusicSource.transform.parent = transform;
				}

				//Creating the audioSource for 2D sounds
				GameObject newSfx2DSource = new GameObject("2D Sfx source ");
				sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
				newSfx2DSource.transform.parent = transform;

				//Set the volumes to saved volumes from PlayerPrefs
				//If there isn't any PlayerPref, we'll use the default value
				sfxVolumePercent = PlayerPrefs.GetFloat("Sfx vol", 1f);
				musicVolumePercent = PlayerPrefs.GetFloat("Music vol", 1f);
			}
		}

		public void SetVolume(float volumePercent, AudioChannel channel)
		{
			switch (channel)
			{
				case AudioChannel.Sfx:
					sfxVolumePercent = volumePercent;
					break;
				case AudioChannel.Music:
					musicVolumePercent = volumePercent;
					break;
				default:
					break;
			}

			//Setting the volumes of musicSources to changed values
			musicSources[0].volume = musicVolumePercent;
			musicSources[1].volume = musicVolumePercent;

			//Saving the change in volume in PlayerPrefs
			PlayerPrefs.SetFloat("Sfx vol", sfxVolumePercent);
			PlayerPrefs.SetFloat("Music vol", musicVolumePercent);

			PlayerPrefs.Save();
		}

		//Method for playing background music
		public void PlayMusic(AudioClip musicClip, float fadeDuration = 1, float optVolume = 1f)
		{
			activeMusicSourceIndex = 1 - activeMusicSourceIndex;
			musicSources[activeMusicSourceIndex].clip = musicClip;
			musicSources[activeMusicSourceIndex].volume = optVolume;
			musicSources[activeMusicSourceIndex].Play();
			StartCoroutine(AnimateMusicCrossfade(fadeDuration));
		}

		//Method for playing sound with a AudioClip and Position
		public void PlaySound(AudioClip clip, Vector3 pos, float optVolume = 1)
		{
			if (clip != null)
				AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * optVolume);
			else
				Debug.LogError("Audio Clip not found, set refences boi!");
		}

		//Method for playing sound with a sound name and Position
		public void PlaySound(string name, Vector3 pos)
		{
			PlaySound(library.GetClipFromName(name), pos);
		}

		//Method for playing 2D sounds
		public void PlaySound2D(string name, float optVolume = 1)
		{
			sfx2DSource.PlayOneShot(library.GetClipFromName(name), sfxVolumePercent * optVolume);
		}

		IEnumerator AnimateMusicCrossfade(float duration)
		{
			float percent = 0f;
			while (percent < 1)
			{
				percent += Time.deltaTime * 1 / duration;
				musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0f, musicVolumePercent, percent);
				musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent, 0f, percent);
				if (musicSources[1 - activeMusicSourceIndex].volume <= 0f)
					musicSources[1 - activeMusicSourceIndex].clip = null;
				yield return null;
			}
		}
	}
}
