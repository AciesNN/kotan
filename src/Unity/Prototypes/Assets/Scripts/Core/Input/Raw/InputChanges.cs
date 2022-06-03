using System.Collections.Generic;
using System.Linq;

namespace Core.Input
{
  public class InputChanges
  {
    IInput input;

    IList<string> actions;

    Dictionary<string, int> axesValues = new Dictionary<string, int>();
    List<string> axes;

    public InputChanges( IInput input, IList<string> actions = null, IList<string> axes = null )
    {
      this.input = input;

      if ( actions != null )
      {
        this.actions = new List<string>( actions );
      }

      if ( axes != null )
      {
        this.axes = new List<string>( axes );
        this.axes.ForEach( a => this.axesValues.Add( a, 0 ) );
      }
    }

    //Update
    public void Update()
    {
      SaveAxes();
    }

    void SaveAxes()
      => axes?.ForEach( a => axesValues[ a ] = GetAxesValue( a ) );

    int GetAxesValue( string name )
      => input.GetAxisRawSign( name );

    //CheckChanges
    public bool CheckChanges()
    => CheckActionChanges() || CheckAxesChanges();

    bool CheckAxesChanges()
      => axesValues?.Any( a => a.Value != GetAxesValue( a.Key ) ) ?? false;

    bool CheckActionChanges()
      => actions?.Any( a => input.GetButtonDown( a ) ) ?? false;
  }
}