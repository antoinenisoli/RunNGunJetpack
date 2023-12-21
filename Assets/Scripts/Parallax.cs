using UnityEngine;
using System.Collections;

// For usage apply the script directly to the element you wish to apply parallaxing
// Based on Brackeys 2D parallaxing script http://brackeys.com/
public class Parallax : MonoBehaviour 
{
	[SerializeField] Vector2 distance = new Vector2(10, 0);
	[SerializeField] Vector2 smoothing = Vector2.one;
	Transform cam; // Camera reference (of its transform)
	Vector3 previousCamPos;
 
	void Awake () 
	{
		cam = Camera.main.transform;
	}
	
	void Update () 
	{
		if (distance.x != 0f) 
		{
			float parallaxX = (previousCamPos.x - cam.position.x) * distance.x;
			Vector3 backgroundTargetPosX = new Vector3(transform.position.x + parallaxX, 
			                                          transform.position.y, 
			                                          transform.position.z);
			
			// Lerp to fade between positions
			transform.position = Vector3.Lerp(transform.position, backgroundTargetPosX, smoothing.x * Time.deltaTime);
		}

		if (distance.y != 0f) 
		{
			float parallaxY = (previousCamPos.y - cam.position.y) * distance.y;
			Vector3 backgroundTargetPosY = new Vector3(transform.position.x, 
			                                           transform.position.y + parallaxY, 
			                                           transform.position.z);
			
			transform.position = Vector3.Lerp(transform.position, backgroundTargetPosY, smoothing.y * Time.deltaTime);
		}

		previousCamPos = cam.position;	
	}
}
