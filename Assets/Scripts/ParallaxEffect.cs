using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //Position for the object    
    Vector2 startingPosition;

    //Start Z value which is vertical float - the further away, the smaller
    float startingZ;

    //Distance that camera has moved from the starting position
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zdistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //If object is in front of target, use nearClipPlane. If behind object, use farClipPlane
    float clippingPlane => (cam.transform.position.z + (zdistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //The further the object from the player, the faster the parallax effect will move. Drag it's Z value closer to the target to make it slower.
    float parallaxFactor => Mathf.Abs(zdistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPostion = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPostion.x, newPostion.y, startingZ);
    }
}
