using Core.Input;
using System.Collections.Generic;

public class PlayerInputPipeline
{
  public const float InputBufferSeconds = 0.02f;
  public const float AxesPressTimeoutSeconds = 0.15f;

  IInput bufferedInput;
  public IInput BufferedInput
    => bufferedInput;

  IPressInput pressInput;
  public IPressInput PressInput 
    => pressInput;

  IRunInput runInput;
  public IRunInput RunInput
    => runInput;

  public void CreatePipeline( IList<string> actions, IList<string> axes )
  {
    bufferedInput = new BufferedInput( RawUnityInput.Default, InputBufferSeconds, actions, axes );
    pressInput = new AxesPressInput( bufferedInput, AxesPressTimeoutSeconds, axes );
    runInput = new AxesRunInput( bufferedInput, pressInput, axes );
  }

  public void UpdateInputs()
  {
    bufferedInput?.Update();
    pressInput?.Update();
    runInput?.Update();
  }
}
