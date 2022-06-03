using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Input
{
  public class AxesRunInput : IRunInput
  {
    IInput input { get; }
    IPressInput pressInput { get; }

    IList<int> currentAxesValues = new List<int>();
    IList<int> previousAxesValues = new List<int>();
    IList<string> axes;
    bool prevIsZero;

    public AxesRunInput( IInput input, IPressInput pressInput, IList<string> axes )
    {
      if ( axes == null || axes.Count <= 0 )
        throw new Exception( $"{nameof( axes )} should not be empty" );

      this.input = input;
      this.pressInput = pressInput;

      this.axes = new List<string>( axes );
      axes.ToList().ForEach( a =>
      {
        currentAxesValues.Add( 0 );
        previousAxesValues.Add( 0 );
      } );
    }

    bool isRun;
    public bool IsRun()
      => isRun;

    public void Update()
    {
      isRun = false;

      if ( pressInput.IsPressed() )
      {
        prevIsZero = false;
        LoadAxesValues( 0, previousAxesValues );
        return;
      }

      LoadAxesValues( input, currentAxesValues, axes );
      var isZero = currentAxesValues.All( a => a == 0 );

      if ( !isZero && prevIsZero && currentAxesValues.SequenceEqual( previousAxesValues ) )
      {
        isRun = true;
      }

      if ( !isZero )
      {
        LoadAxesValues( currentAxesValues, previousAxesValues );
      }
      prevIsZero = isZero;
    }

    static void LoadAxesValues( IList<int> src, IList<int> dst)
    {
      for ( int i = 0; i < dst.Count && i < src.Count; i++ )
      {
        dst[i] = src[i];
      }
    }

    static void LoadAxesValues( int val, IList<int> dst )
    {
      for ( int i = 0; i < dst.Count; i++ )
      {
        dst[ i ] = val;
      }
    }

    static void LoadAxesValues( IInput input, IList<int> axesValues, IList<string> axes )
    {
      for ( int i = 0; i < axesValues.Count && i < axes.Count; i++ )
      {
        axesValues[i] = input?.GetAxisRawSign( axes[i] ) ?? 0;
      }
    }
  }
}