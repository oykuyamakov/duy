using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using Roro.Scripts.Serialization;
using Roro.Scripts.Utility;
using SceneManagement;
using SceneManagement.EventImplementations;
using Sounds;
using Sounds.Helpers;
using UnityCommon.Singletons;
using UnityCommon.Variables;
using UnityEngine;
using Utility;

namespace Roro.Scripts.Sounds.Core
{
	[DefaultExecutionOrder(ExecOrder.SoundManager)]
	[RequireComponent(typeof(AudioSource))]
	public class SoundManager : SingletonBehaviour<SoundManager>
	{
		
		[SerializeField]
		private Transform m_LoopSoundParent;

		[SerializeField] 
		private AudioSource m_MainSong;

		private Sound m_CurrentMainSound;

		private Tween m_MainAudioTweener;

		private List<AudioSource> m_LoopSources => m_LoopSoundParent.GetComponents<AudioSource>().ToList();

		private List<AudioSource> m_AudioSources => GetComponents<AudioSource>().ToList();
		
		private SerializationWizard m_SerializationWizard = SerializationWizard.Default;

		private int m_SourceIndex;
		private int m_LoopSourceIndex = 0;
		private int m_AvailableSourceCount;
		
		private float m_SfxVolume => m_SerializationWizard.GetInt("Sfx Volume", 10) / 10f;
		private float m_MusicVolume => m_SerializationWizard.GetInt("Music Volume", 10) / 10f;
		
		private void Awake()
		{
			if(!SetupInstance(false))
				return;

			m_SourceIndex = 0;
			m_LoopSourceIndex = 0;
			
			GEM.AddListener<SoundPlayEvent>(OnSoundPlayEvent);
			GEM.AddListener<SoundStopEvent>(OnSoundStopEvent);
		}

		public void OnMusicVolumeChange()
		{
			var currentVolume = m_CurrentMainSound != null ? m_CurrentMainSound.Volume : 1;
			m_MainSong.volume = m_MusicVolume * currentVolume;
		}

		private void OnSoundPlayEvent(SoundPlayEvent evt)
		{
			if (evt.MainSong)
			{
				m_CurrentMainSound = evt.Sound;
				TryPlayMainSong(evt.Sound, m_MusicVolume);
				return;
			}
			if (evt.Loop)
			{
				PlayLoop(evt.Sound, m_MusicVolume);
				evt.LoopIndex = m_LoopSourceIndex - 1;
			}
			else
			{
				PlayOneShot(evt.Sound, m_SfxVolume);
			}
		}
		
		private void TryPlayMainSong(Sound sound, float volume = 1f, float pitch = 1f)
		{
			m_MainAudioTweener?.Kill();
			m_MainAudioTweener = m_MainSong.DOFade(0, 0.1f).OnComplete( () => PlayMainSong(sound,volume,pitch));
		}
		
		private void PlayMainSong(Sound sound, float volume = 1f, float pitch = 1f)
		{
			m_MainSong.loop = true;
			m_MainSong.volume = volume;
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			m_MainSong.Play(sound, volume, pitch);
		}

		private void OnSoundStopEvent(SoundStopEvent evt)
		{
			StopSound(evt.LoopIndex);
		}

		public void StopSound(int index)
		{
			m_LoopSources[index].Stop();
		}

		// public void OnSceneLoadEvent(OnSceneChangedEvent evt)
		// {
		// 	if (!evt.SceneController.IsPermanent)
		// 	{
		// 		Reset();
		// 	}
		// }
		
		private AudioSource GetSource()
		{
			var src = m_AudioSources[m_SourceIndex++];
			m_SourceIndex %= m_AudioSources.Count;
			return src;
		}

		private AudioSource GetLoopSource()
		{
			var src = m_LoopSources[m_LoopSourceIndex++];
			m_LoopSourceIndex %= m_AudioSources.Count;
			return src;
		}
		
		public void Reset()
		{
			Debug.Log("Reset");
			
			// m_AudioSources.ForEach(source =>
			// {
			// 	if (source.isPlaying)
			// 	{
			// 		source.DOFade(0, 0.1f).OnComplete(() =>source.loop = false);
			// 	}
			//
			// 	m_AvailableSourceCount = m_AudioSources.Count;
			// });
		}

		public void PlayOneShot(Sound sound, float volume = 1f, float pitch = 1f)
		{
			// if (m_SoundsDisabled.Value)
			// 	return;
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				//Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			var src = GetSource();
			src.PlayOneShot(sound, volume, pitch);
		}
		public void PlayLoop(Sound sound, float volume = 1f, float pitch = 1f)
		{
			// if (m_SoundsDisabled.Value)
			// 	return;
			
			if (!sound || !sound.Clip || sound.Volume < 1e-2f)
			{
				//Debug.Log($"Ignoring sound {sound.name}");
				return;
			}

			var src = GetLoopSource();
			src.PlayOneShot(sound, volume, pitch);
		}
		
		
#if UNITY_EDITOR

		public void GetSounds()
		{
			
		}
#endif

		[Serializable]
		public class SoundPair
		{
			public int id;
			public Sound sound;
		}

	}
}
