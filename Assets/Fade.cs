using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	public SpriteRenderer image;
	public float duration, startingFade, red, green, blue;
	//public bool brighten;
	float fading;

	void Start()
	{
		/*if (brighten)
			fading = (startingFade - 1f) / duration;
		else fading = startingFade / duration;*/
		fading = startingFade / duration;
	}

	void Update()
	{
		startingFade -= fading * Time.deltaTime;
		image.color = new Color(red, green, blue, startingFade);
	}
}
