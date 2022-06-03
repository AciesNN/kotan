using Core.Utils;
using System.Collections.Generic;

namespace Core.Input
{
  public class AxesPressInput : IPressInput
  {
    Timer timer { get; }
    InputChanges inputChanges { get; }

    public AxesPressInput( IInput input, float seconds, IList<string> axes )
    {
      timer = DI.Get<ITimerFactory>().New( seconds );
      inputChanges = new InputChanges( input, null, axes );
    }

    public bool IsPressed()
      => timer.Check();

    public void Update()
    {
      if ( inputChanges.CheckChanges() )
      {
        timer.Off();
        timer.On();
      }

      inputChanges.Update();
    }
  }
}