using System;
using System.Linq;
using System.Collections.Generic;

public class PlayerInputNamesGenerator<A> where A : Enum
{
  readonly IList<string> actions; 
  readonly IList<string> axes; 

  public PlayerInputNamesGenerator( string postfix )
  {
    actions = Array.ConvertAll( ( A[] )typeof( A ).GetEnumValues(), new Converter<A, string>( e => e.ToString() ) )
      .Select( a => $"{a}{postfix}").ToList();

    axes = new List<string>()
    {
      $"Horizontal{postfix}",
      $"Vertical{postfix}",
    };
  }

  public IList<string> GetActions()
    => actions;

  public IList<string> GetAxes()
    => axes;
}