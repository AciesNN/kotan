using Core.Input;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public enum Actions
  {
    Jump,
    Fire1,
  }

  [SerializeField]
  string postfix;
  
  [SerializeField]
  Player player;

  PlayerInputPipeline playerInputPipeline;

  private void Awake()
  {

    var playerInputNamesGenerator = new PlayerInputNamesGenerator<Actions>( postfix );
    var actions = playerInputNamesGenerator.GetActions();
    var axes = playerInputNamesGenerator.GetAxes();

    playerInputPipeline = new PlayerInputPipeline();
    playerInputPipeline.CreatePipeline( actions, axes );
  }

  void Update()
  {
    playerInputPipeline.UpdateInputs();
    var input = playerInputPipeline.BufferedInput;
    var runInput = playerInputPipeline.RunInput;

    if ( input == null )
    {
      return;
    }

    /*TODO*/
    var fire = Actions.Fire1.ToString();
    if ( input.GetButtonDown( fire ) && player )
    { 
      player.Hit();
    }

    /*TODO*/ var h = "Horizontal";
    /*TODO*/ var v = "Vertical";
    var hm = input.GetAxisRawSign(h);
    var vm = input.GetAxisRawSign(v);
    var r = runInput?.IsRun() ?? false;

    player.Move(new Vector2( hm, vm ), r);

    var pressInput = playerInputPipeline.PressInput;
    if ( pressInput != null )
    {
      var isPressed = pressInput.IsPressed() && ( hm != 0 || vm != 0 );
      player.SetPressState( isPressed );
    }
  }
}
