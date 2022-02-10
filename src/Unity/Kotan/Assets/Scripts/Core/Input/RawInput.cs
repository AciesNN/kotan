using UnityEngine;

namespace Core.Input
{
    /// <summary>
    /// wrapper upon an old Unity Input system
    /// </summary>
    public class RawInput
    {
        string HorizontalAxis => $"Horizontal{axisName}";
        string VerticalAxis => $"Vertical{axisName}";

        private readonly string axisName = "";
        public RawInput(string axisName)
        {
            this.axisName = axisName;
        }

        #region Interface
        /// <summary>
        /// Should be called on Unity thread only?
        /// </summary>
        /// <returns></returns>
        public Vector2Int GetJoystickPositionInt() 
            => new Vector2Int(
                SignOrZero(UnityEngine.Input.GetAxisRaw(HorizontalAxis)),
                SignOrZero(UnityEngine.Input.GetAxisRaw(VerticalAxis))
            );

        public bool GetButtonDown(string buttonName)
            => UnityEngine.Input.GetButtonDown($"{buttonName}{axisName}");
        #endregion

        /// <summary>
        /// Should be called on Unity thread only?
        /// </summary>
        /// <returns></returns>


        #region Implementation
        private int SignOrZero(float val)
        {
            return Mathf.Approximately(val, 0) ?
                0 : val > 0 ? 1 : -1;
        }
        #endregion
    }
}