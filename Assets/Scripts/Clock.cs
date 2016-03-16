using UnityEngine;
using System.Collections;
using System;

public class Clock : MonoBehaviour, ITarget
{
    public TargetManager targetManager { get; set; }

    [SerializeField]
    GameObject hourHand;

    [SerializeField]
    GameObject minuteHand;

    [SerializeField]
    GameObject secondHand;

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // If clock collides with a projectile, then we trigger a hit
        IProjectile projectile = collider.gameObject.GetComponent<IProjectile>();
        if (projectile != null)
        {
            targetManager.TargetHit(gameObject);
        }
    }

    /// <summary>
    /// Rotate each available clock hand to the appropriate position based on the current time
    /// </summary>
    void UpdateTime()
    {
        if (hourHand != null)
        {
            Quaternion currentHour = Quaternion.Euler(0, 0, -HourToDegree());
            hourHand.transform.rotation = Quaternion.Slerp(hourHand.transform.rotation, currentHour, Time.deltaTime * 2.0f);
        }

        if (minuteHand != null)
        {
            Quaternion currentHour = Quaternion.Euler(0, 0, -MinuteToDegree());
            minuteHand.transform.rotation = Quaternion.Slerp(minuteHand.transform.rotation, currentHour, Time.deltaTime * 2.0f);
        }

        if (secondHand != null)
        {
            Quaternion currentHour = Quaternion.Euler(0, 0, -SecondToDegree());
            secondHand.transform.rotation = Quaternion.Slerp(secondHand.transform.rotation, currentHour, Time.deltaTime * 2.0f);
        }
    }

    /// <summary>
    /// Returns the position of the hour hand on a 360 degree circle
    /// </summary>
    /// <param name="useMinutes">Optional flag to choose whether to advance the hour hand a fraction of an hour based on the current minute. (I.e. a third of the way between hours 1 and 2 when the current minute is 20)</param>
    private float HourToDegree(bool useMinutes = true)
    {
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;

        float exactHour = ((hour * 360) / 12);

        if (useMinutes)
        {
            float minutesOffset = ((minute * 30) / 60);
            return exactHour + minutesOffset;
        }

        return exactHour;
    }

    /// <summary>
    /// Returns the position of the minutes hand on a 360 degree circle
    /// </summary>
    /// <param name="useSeconds">Optional flag to choose whether to advance the minutes hand a fraction of a minute based on the current second. (I.e. right between minute 1 and minute 2 when current seconds is 30)</param>
    private float MinuteToDegree(bool useSeconds = true)
    {
        int minute = DateTime.Now.Minute;
        int second = DateTime.Now.Second;

        float exactMinute = ((minute * 360) / 60);

        if (useSeconds)
        {
            float minutesOffset = ((minute * 6) / 60);
            return exactMinute + minutesOffset;
        }

        return exactMinute;
    }

    /// <summary>
    /// Returns the position of the seconds hand on a 360 degree circle
    /// </summary>
    private float SecondToDegree()
    {
        int second = DateTime.Now.Second;

        float exactSecond = ((second * 360) / 60);

        return exactSecond;
    }
}
