using UnityEngine;

namespace Core.Utils
{
  //can be set ON, then CHECK if time's passed and turned OFF
  public class Timer
  {
    float seconds;
    internal Timer( float seconds )
    {
      this.seconds = seconds;
    }

    bool on;
    float time;

    //On
    public void On()
    {
      if ( on )
        return;
      on = true;
      SaveTime();
    }

    void SaveTime()
    {
      time = currentTime;
    }

    float currentTime
      => Time.time;

    //Off
    public void Off()
      => on = false;

    //Check
    public bool Check()
      => on && CheckTime();

    bool CheckTime()
      => currentTime - time >= seconds;
  }

  //factory
  public interface ITimerFactory
  {
    Timer New( float seconds );
  }

  [RegisterDI( typeof( ITimerFactory ) )]
  public class TimerFactory : ITimerFactory
  {
    public Timer New( float seconds )
      => new Timer( seconds );
  }
}