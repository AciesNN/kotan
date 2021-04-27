using UI;

namespace Unit.UnitStateInputLogic
{
    public abstract class BaseUnitStateInputLogic
    {
        protected virtual InputAction? checkAction => null;
        protected virtual bool? checkInputForce => null;
        
        public bool? CheckAmimationComplete;
        protected virtual bool? checkAmimationComplete => CheckAmimationComplete;
        
        protected virtual UnitState? newState => null;
        protected virtual bool setDir => false;

        public virtual bool ProcessInput()
        {
            inputState = input.GetInputState();

            if (!CheckAnimationComplete()) {
                return false;
            }

            if (!CheckInputForce()) {
                return false;
            }
            
            if (!CheckInputAction()) {
                return false;
            }

            if (!CheckCondition()) {
                return false;
            }

            SetUnitState();
            SetUnitDirection();
            ProcessImpl();
            return true;
        }

        protected virtual bool CheckCondition() => true;
        protected virtual void ProcessImpl() { }

        #region DI
        //weird DI pattern? TODO?
        protected BufferedStatedInput input;
        protected Unit unit;
        protected InputState inputState;
        public void SetCurrentData(Unit unit, BufferedStatedInput input)
        {
            this.unit = unit;
            this.input = input;

        }
        #endregion

        #region Checks - move to strategy classes
        protected bool CheckAnimationComplete()
        {
            return checkAmimationComplete.HasValue ? checkAmimationComplete.Value == unit.IsAnimationComplete : true;
        }

        protected bool CheckUnitHitDetected()
        {
            return unit.HitDetected;
        }

        protected bool CheckInputAction()
        {
            return checkAction.HasValue ? inputState.action.Equals(checkAction.Value) : true;
        }

        protected bool CheckInputForce()
        {
            return checkInputForce.HasValue ? checkInputForce == inputState.force : true;
        }

        protected bool CheckInputDirecton(bool? xNotZero = null, bool? yNotZero = null)
        {
            bool checkDirX = xNotZero.HasValue ? (xNotZero.Value ? inputState.dir.x != 0: inputState.dir.x == 0) : true;
            bool checkDirY = yNotZero.HasValue ? (yNotZero.Value ? inputState.dir.y != 0 : inputState.dir.y == 0) : true;
            return checkDirX && checkDirY;
        }
        #endregion

        #region Actions - move to strategy classes
        private void SetUnitState()
        {
            if (newState.HasValue) {
                unit.SetState(newState.Value);
            }
        }
        
        private void SetUnitDirection()
        {
            if (setDir) {
                unit.SetDirection(inputState.dir);
            }
        }
        
        protected void StopUnit()
        {
            unit.StopMove();
        }

        protected void UnitMove()
        {
            unit.Move(inputState.dir, inputState.force);
        }
        
        protected void UnitJump()
        {
            unit.Jump(inputState.dir, inputState.force);
        }

        protected void ResetForce()
        {
            input.ResetForce();
        }

        protected void BufferInputState()
        {
            input.AddBuffer();
        }
        #endregion
    }
}