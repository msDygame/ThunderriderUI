using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
	
public class JSK_SoundProcess : MonoBehaviour
{
#if _DebugMode_
	public  static float		m_fDefaultSoundVolume 	= 0f;
	public  static float 		m_fDefaultMusicVolume	= 0f;
#else
	public  static float		m_fDefaultSoundVolume 	= 0.8f;
	public  static float 		m_fDefaultMusicVolume	= 0.4f;
#endif
	public  static float 		m_fSoundVolume        	= m_fDefaultSoundVolume;
	public  static float 		m_fMusicVolume			= m_fDefaultMusicVolume;
	//-------------------------------------------------
	private static int 			m_iAudioPlayCount 		= 10;
	private static int 			m_iPlayerCount 	 		= 4;
	private static Hashtable 	m_hsSoundObjDB    		= new Hashtable();
	private static ArrayList 	m_AllAudios 			= new ArrayList();
	private static ArrayList 	m_aAudioPlayList  		= new ArrayList();
	private static ArrayList 	m_aMusicPlayList  		= new ArrayList();
	private static ArrayList 	m_SoundNameQueue  		= new ArrayList();
	private static ArrayList 	m_PlayerStateList 		= new ArrayList();
	private static AudioSource 	m_SoundListAudio 		= null;
	private static string 		m_szNowMusicName  		= "";
	private static string 		m_szSetMusicName  		= "";
	private static int 	  		m_iNowMusicIndex  		= 0;
	private static float  		m_iNowMusicVolume 		= 0;
	private static float  		m_fGradualChange  		= 0;
	private static string 		m_sSoundPath			= "JSK/Sound/";
	private static string 		m_sMusicPath			= "JSK/Music/";

	public static void LoadConfig()
	{
		//m_fSoundVolume = PlayerPrefs.GetFloat("JSK_SoundVolume", m_fDefaultSoundVolume);
		//m_fMusicVolume = PlayerPrefs.GetFloat("JSK_MusicVolume", m_fDefaultMusicVolume);
	}

	public static void SaveConfig()
	{
		//PlayerPrefs.SetFloat("JSK_SoundVolume", m_fSoundVolume);
		//PlayerPrefs.SetFloat("JSK_MusicVolume", m_fMusicVolume);
	}
	
	public static void StopAllAudios()
    {
        foreach( AudioSource audio in m_AllAudios )
		{
			audio.Stop();
		}
    }
	
	public static void PlaySound( string szSoundName )
	{
		if( szSoundName == "" )
			return;
		
		AudioClip ClipData = GetSoundClip(m_sSoundPath, szSoundName);
	
		if( ClipData == null )
			return;
			
		for( int i = 0; i < m_aAudioPlayList.Count; i++ )
		{
			AudioSource AudioObj = m_aAudioPlayList[i] as AudioSource;
			if( AudioObj.enabled == false )
			{
				AudioObj.enabled = true;
		    	AudioObj.clip = ClipData;
				AudioObj.volume = m_fSoundVolume;
    	    	AudioObj.Play();
    	    	return;
			}
		}
	}
 	//若是要播放連續多個聲音組合(用逗號分隔),則將其餘音效名稱放到queue.
	public static void PlaySoundList( string szSoundNameList )
	{
		if( szSoundNameList == "" )
			return;
	
		AudioClip ClipData = null;
		
		if( szSoundNameList.IndexOf(",") >= 0 )
		{
			m_SoundNameQueue.Clear();
			string[] nameList = szSoundNameList.Split(","[0]);
		
			for( int i = 0; i < nameList.Length; i++ )
			{
				ClipData = GetSoundClip(m_sSoundPath, nameList[i]);
				if( ClipData != null )
					m_SoundNameQueue.Add(ClipData);
			}
		}
		if( m_SoundNameQueue.Count > 0 )
		{
			ClipData = m_SoundNameQueue[0] as AudioClip;
			m_SoundNameQueue.RemoveAt(0);
			m_SoundListAudio.clip = ClipData;
			m_SoundListAudio.volume = m_fSoundVolume;
    		m_SoundListAudio.Play();
		}
	}
	
	public static void StopSoundList()
	{
		m_SoundListAudio.Stop();
		m_SoundNameQueue.Clear();
	}
	//判断声音队列是否播放完毕.
	public static bool IsSoundListPlaying()
	{
		if( m_SoundNameQueue.Count > 0 )
			return true;
		else if( m_SoundListAudio.isPlaying )
			return true;
		return false;
	}

	public static void StopSound( string szSoundName )
	{
		if( szSoundName == "" )
			return;
		
		AudioClip ClipData = GetSoundClip(m_sSoundPath, szSoundName);
	
		if( ClipData == null )
			return;
			
		for( int i = 0; i < m_aAudioPlayList.Count; i++ )
		{
			AudioSource AudioObj = m_aAudioPlayList[i] as AudioSource;
			if( AudioObj.enabled == true && AudioObj.clip == ClipData && AudioObj.isPlaying == true )
			{
				AudioObj.Stop();
				AudioObj.enabled = false;
    	    	return;
			}
		}
	}

	public static void PlayStateSound( int index, string szSoundName )
	{
		if( szSoundName == "" )
			return;
		
		AudioClip ClipData = GetSoundClip(m_sSoundPath, szSoundName);
	
		if( ClipData == null )
			return;
			
		AudioSource AudioObj = m_PlayerStateList[index] as AudioSource;
		AudioObj.clip = ClipData;
		AudioObj.volume = m_fSoundVolume;
    	AudioObj.Play();
	}

	public static void StopStateSound( int index )
	{
		AudioSource AudioObj = m_PlayerStateList[index] as AudioSource;
		AudioObj.Stop();
	}
	
	public static void StopAllStateSound()
	{
		for( int i = 0; i < m_PlayerStateList.Count; i++ )
		{
			AudioSource AudioObj = m_PlayerStateList[i] as AudioSource;
			AudioObj.Stop();
		}
	}
	
	public static void PlayMusic( string szMusicName )
	{
		if( szMusicName == "" )
			return;
		
		if( m_szNowMusicName == szMusicName )
			return;
		
		m_szSetMusicName = szMusicName;
    	m_fGradualChange = 1.0f;
    
    	AudioSource AudioObj = m_aMusicPlayList[m_iNowMusicIndex] as AudioSource;
    
    	m_iNowMusicVolume = AudioObj.volume;
    
    	if( m_iNowMusicIndex == 0 )
    		AudioObj = m_aMusicPlayList[1] as AudioSource;
    	else
    		AudioObj = m_aMusicPlayList[0] as AudioSource;
    	
    	AudioObj.clip = GetSoundClip(m_sMusicPath, m_szSetMusicName);
    	AudioObj.Play();
    	AudioObj.volume = 0;
	}

	public static void ChangeMusicVolume()
	{
    	AudioSource AudioObj = m_aMusicPlayList[m_iNowMusicIndex] as AudioSource;
    	AudioObj.volume = m_fMusicVolume;
	}
	
	public static void StopAllSound()
	{
		for( int i = 0; i < m_aAudioPlayList.Count; i++ )
		{
			AudioSource AudioObj = m_aAudioPlayList[i] as AudioSource;
			AudioObj.Stop();
			AudioObj.enabled = false;
		}
	}

	void Awake()
	{
		Debug.Log("JSK_SoundProcess Awake");
		
		m_AllAudios.Clear();
		m_hsSoundObjDB.Clear();
		m_aAudioPlayList.Clear();
		m_aMusicPlayList.Clear();
		m_SoundNameQueue.Clear();
		m_PlayerStateList.Clear();
		m_SoundListAudio = null;
		
		int i = 0;
		for( i = 0; i < m_iAudioPlayCount; i++ )
		{
    		AudioSource AudioObj = gameObject.AddComponent<AudioSource>();
			AudioObj.loop = false;
			AudioObj.enabled = false;
			AudioObj.panLevel = 0;//2d音效.
        	m_aAudioPlayList.Add(AudioObj);
			m_AllAudios.Add(AudioObj);
		}
		
		for( i = 0; i < 2; i++ )
		{
			AudioSource MusicObj = gameObject.AddComponent<AudioSource>();
        	MusicObj.loop = true;
			MusicObj.panLevel = 0;//2d音效.
        	m_aMusicPlayList.Add(MusicObj);
			m_AllAudios.Add(MusicObj);
		}
	
		for( i = 0; i < m_iPlayerCount; i++ )
		{
    		AudioSource AudioObj2 = gameObject.AddComponent<AudioSource>();
			AudioObj2.loop = true;
			AudioObj2.panLevel = 0;//2d音效.
        	m_PlayerStateList.Add(AudioObj2);
			m_AllAudios.Add(AudioObj2);
		}
	
		m_SoundListAudio = gameObject.AddComponent<AudioSource>();
		m_SoundListAudio.loop = false;
		m_SoundListAudio.panLevel = 0;//2d音效.
		m_AllAudios.Add(m_SoundListAudio);
	}

	void Update() 
	{
		AudioListUpdate();
		AudioUpdate();
   	 	MusicUpdate();
	}

	private static AudioClip GetSoundClip( string type, string szSoundName )
	{
		AudioClip ClipData = null;
    	if( !m_hsSoundObjDB.ContainsKey(szSoundName) )
		{
			ClipData = (AudioClip)Resources.Load(type + szSoundName);
    		if( ClipData != null )
    			m_hsSoundObjDB.Add(szSoundName,ClipData);
    		else
    	     	return ClipData;
    	}
    	else
    	{
    		ClipData = m_hsSoundObjDB[szSoundName] as AudioClip;
    	}
    	return ClipData;
	}

	void MusicUpdate()
	{
		if( m_szSetMusicName != m_szNowMusicName )
    	{
    		int iSID = 0;
    		if( m_iNowMusicIndex == 0 )
    			iSID = 1;
    	
    		AudioSource lastAudioObj = m_aMusicPlayList[m_iNowMusicIndex] as AudioSource;
    		AudioSource curAudioObj  = m_aMusicPlayList[iSID] as AudioSource;
    		  
			m_fGradualChange -= (Time.deltaTime * 0.8f);
		
			if( m_fGradualChange > 0.0f )
			{
				curAudioObj.volume = m_fMusicVolume * (1.0f - m_fGradualChange);
            	lastAudioObj.volume = m_iNowMusicVolume * m_fGradualChange;
        	}
        	else
        	{
        		if( curAudioObj.volume != m_fMusicVolume )
        			curAudioObj.volume  = m_fMusicVolume;
			  
				if( lastAudioObj.volume != 0 )  	  
                	lastAudioObj.volume = 0.0f;
                
            	lastAudioObj.Stop();
 				m_iNowMusicIndex = iSID;
    			m_szNowMusicName = m_szSetMusicName ;
        	}
		}
	}

	void AudioUpdate()
	{
    	for( int i = 0; i < m_aAudioPlayList.Count; i++ )
		{
			AudioSource AudioObj = m_aAudioPlayList[i] as AudioSource;
			if( AudioObj.enabled == true && AudioObj.isPlaying == false )
			{
				AudioObj.enabled = false;
			}
		}
	}

	void AudioListUpdate()
	{
		if( m_SoundListAudio.isPlaying )
			return;
		
		if( m_SoundNameQueue.Count > 0 )
		{
			AudioClip ClipData = m_SoundNameQueue[0] as AudioClip;
			m_SoundNameQueue.RemoveAt(0);
			m_SoundListAudio.clip = ClipData;
			m_SoundListAudio.volume = m_fSoundVolume;
    		m_SoundListAudio.Play();
		}
	}
	
	void OnLevelWasLoaded( int level )
	{
		StopSoundList();
		StopAllStateSound();
	}
	
}
