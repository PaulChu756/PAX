using UnityEngine;
using System.Collections;

/// <summary>
/// Paul Chu's Helicopter GUI
/// </summary>
public class AircraftGUI : MonoBehaviour
{
    void OnGUI()
    {
        RaycastHit groundHit;

        Physics.Raycast(player.transform.position - Vector3.up, -Vector3.up, out groundHit);
        helicopterThrottle = player.GetComponent<Aircraft>().rotorVel;

        // Setting HelicopterThrottle = 0.0f stops the errors because everytime it resets it instead of reaching 1.
        helicopterThrottle = 0.0f;
        GUI.Label(new Rect(0, 0, 128, 128), altimeter);
        GUI.Label(new Rect(0, 128, 128, 128), throttle[(int)(helicopterThrottle * 10)]);
        GUI.Label(new Rect(40, 40, 256, 256), Mathf.Round(groundHit.distance) + " METERS ");
        GUI.Label(new Rect(20, 182, 256, 256), " ENGINE ");
        //Debug.Log(helicopterThrottle);
    }

    public GameObject player;
    private float helicopterThrottle;
    public Texture altimeter;
    public Texture[] throttle;
}
