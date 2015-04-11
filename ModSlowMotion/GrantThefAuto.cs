using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using SlimDX.XInput;

namespace ModSlowMotion
{  
    public class GrantThefAuto : Script
    {
        #region DECLARE VAR'S
        private readonly Controller GamePad = new Controller(UserIndex.One);
        private Boolean IsGamePadConnected = false;
        
        private bool slowMotionEnabled = false;
        private float slowMotionScaleDefault = 1.0f;
        private float slowMotionScaleSlow = 0.3f;

        private Keys keyEnabledSlowMotionKeyBoard = Keys.Pause;

        private GamepadButtonFlags keyEnabledSlowMotionXboxController = GamepadButtonFlags.DPadDown;
        #endregion

        #region CONSTRUCTOR
        public GrantThefAuto() {
            // Show to player how is the key to enable/disable
            try
            {
                Log.i(String.Format("Press {0} or Button DPadDown to enable the SlowMotion.", util.GetKeyName(keyEnabledSlowMotionKeyBoard)), "");
            }
            catch (NotSupportedException e)
            {
                keyEnabledSlowMotionKeyBoard = Keys.Pause;
                Log.i(String.Format("Press Pause or Button DPadDown to enable the SlowMotion."), "");
            }

            this.Interval = 2000;
            // This event check if the xbox controller is connected.
            this.Tick += new EventHandler(this.xboxControllerCheckState);

            // This event is to enabled/desabled the SlowMotion
            // on user press the key on keyboard
            this.BindKey(keyEnabledSlowMotionKeyBoard, this.toggleSlowMotion);

            // This event is to enabled/desabled the SlowMotion
            // on user press the key on xboxController
            this.Tick += new EventHandler(this.toggleSlowMotion);

            // This event is the slowMotion Function
            this.Tick += new EventHandler(this.slowMotion);                        
            Wait(5000);
        }
        #endregion

        #region EVENT'S HANDLER
        private void xboxControllerCheckState(object sender, EventArgs e)
        {
            if (this.GamePad.IsConnected)
            {
                this.IsGamePadConnected = true;
            }
            else
            {
                this.IsGamePadConnected = false;
            }
        }

        private void toggleSlowMotion()
        {
            slowMotionEnabled = !slowMotionEnabled;   
        }

        private void toggleSlowMotion(object sender, EventArgs e)
        {
            if (this.IsGamePadConnected)
            {
                if (this.GamePad.GetState().Gamepad.Buttons.CompareTo(keyEnabledSlowMotionXboxController) == 0)
                {
                    slowMotionEnabled = !slowMotionEnabled;
                }
            }
        }

        private void slowMotion(object sender, EventArgs e)
        {
            if (slowMotionEnabled)
            {
                Game.TimeScale = this.slowMotionScaleSlow;  
            }
            else
            {
                Game.TimeScale = this.slowMotionScaleDefault;
            }
        }
        #endregion
    }
}
