using UnityEngine;
using UnityEngine.UI;

// Contains necessary information to track progress of each game
public class TimeTrack : MonoBehaviour
{
    Text time; // In game time track
    private float startTime;

    // Get all info and gameobj necessary for tracking everything
    private void Start()
    {
        startTime = Time.time;
        time = transform.Find("BattleUI").Find("Timer").GetComponent<Text>();
    }

    // Gold and time will update every frame
    private void FixedUpdate()
    {
        // Also update time
        int min = (int)((Time.time - startTime) / 60);
        int sec = (int)((Time.time - startTime) % 60);
        time.text = ((min / 10 == 0)? "0" : "") + min + ":" + ((sec / 10 == 0) ? "0" : "") + sec;
    }

}
