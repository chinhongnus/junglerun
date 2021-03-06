﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvents : MonoBehaviour {

	public CanvasGroup startGroup;
	public CanvasGroup endGroup;

	const float fadeTime = 0.5f;
	const float fadePerSecond = 1.0f / fadeTime;
	float fadeFactor = 0.0f;

	bool hasTriggeredEnd = false;
	bool hasTriggeredMusicEnd = false;

	const float musicFadeTime = 2.0f;
	const float musicFadePerSecond = 1.0f / musicFadeTime;
	float musicVolume = 1.0f;

	float timeBeforeFade = 2.0f;

	Animator animator;

	public AudioSource audioSource;
	AudioSource collectAudioSource;
	Animator collectAnimator;

	GameObject playerObject;
	const float thrustX = 10.0f;
	const float thrustY = 20.0f;

	// Use this for initialization
	void Start () 
	{
		animator = endGroup.gameObject.GetComponent<Animator>();
		collectAudioSource = gameObject.GetComponent<AudioSource>();
		collectAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!hasTriggeredEnd)
		{
			return;
		}

		if (timeBeforeFade > 0.0f)
		{
			timeBeforeFade -= Time.deltaTime;
			return;
		}

		if (startGroup == null || endGroup == null)
		{
			return;
		}

		if (hasTriggeredMusicEnd)
		{
			if (audioSource != null)
			{
				musicVolume -= musicFadePerSecond * Time.deltaTime;
				if (musicVolume < 0.0f)
				{
					musicVolume = 0.0f;
				}
				audioSource.volume = musicVolume;
			}
		}

		if (fadeFactor >= 1.0f)
		{
			return;
		}

		fadeFactor += fadePerSecond * Time.deltaTime;

		if (fadeFactor > 1.0f)
		{
			fadeFactor = 1.0f;
			animator.SetBool("DisplayTexts", true);
			hasTriggeredMusicEnd = true;

			if (playerObject != null)
			{
				playerObject.SetActive(false);
			}
		}

		startGroup.alpha = 1.0f - fadeFactor;
		endGroup.alpha = fadeFactor;
	}

	void OnTriggerEnter2D(Collider2D otherGameObject)
	{
		if (otherGameObject.tag == "Player")
		{
			Animator playerAnimator = otherGameObject.GetComponent<Animator>();
			playerAnimator.SetBool("IsOnJetpack", true);
			hasTriggeredEnd = true;
			collectAudioSource.Play();
			collectAnimator.SetBool("Collected", true);

			playerObject = otherGameObject.gameObject;
			playerObject.GetComponent<ApplyForce>().ApplyThrust(thrustX, thrustY);
		}
	}
}
