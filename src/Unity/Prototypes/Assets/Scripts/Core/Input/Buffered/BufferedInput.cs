using Core;
using Core.Input;
using Core.Utils;
using System.Collections.Generic;

public class BufferedInput : IInput
{
  IInput input { get; }
  Timer timer { get; }
  InputChanges inputChanges { get; }

  Dictionary<string, bool> actionValues = new Dictionary<string, bool>();
  List<string> actions { get; }

  Dictionary<string, int> axesValues = new Dictionary<string, int>();
  List<string> axes { get; }

  bool updatedThisFrame;

  public BufferedInput( IInput input, float seconds, IList<string> actions = null, IList<string> axes = null )
  {
    this.input = input;
    timer = DI.Get<ITimerFactory>().New( seconds );
    inputChanges = new InputChanges( input, actions, axes );

    if ( actions != null )
    {
      this.actions = new List<string>( actions );
    }

    if ( axes != null )
    {
      this.axes = new List<string>( axes );
    }

    ClearActionBuffer();
  }

  public void Update()
  {
    if ( updatedThisFrame ) //previous frame
    {
      ClearActionBuffer();
    }

    UpdateActionBuffer();

    updatedThisFrame = timer.Check();
    var changesThisFrame = inputChanges.CheckChanges();
    if ( !updatedThisFrame && changesThisFrame )
    {
      timer.On();
    }

    inputChanges.Update();

    if ( updatedThisFrame )
    {
      timer.Off();
      SaveAxesValues();
    }
  }

  void UpdateActionBuffer()
    => actions?.ForEach( a => actionValues[a] = actionValues[a] || input.GetButtonDown( a ) );

  void SaveAxesValues()
    => axes?.ForEach( a => axesValues[a] = input.GetAxisRawSign( a ) );

  void ClearActionBuffer()
    => actions?.ForEach( a => actionValues[a] = false );

  public int GetAxisRawSign( string name )
    => updatedThisFrame ? input.GetAxisRawSign( name ) : ( axesValues.ContainsKey( name ) ? axesValues[name]: 0 );

  public bool GetButtonDown( string name )
    => updatedThisFrame ? ( actionValues.ContainsKey( name ) ? actionValues[ name ] : false ) : false;
}
