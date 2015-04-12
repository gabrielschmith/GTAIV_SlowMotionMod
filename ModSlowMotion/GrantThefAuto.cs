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
        private readonly Controller GamePad     = new Controller(UserIndex.One);
        private Boolean IsGamePadConnected      = false;
        
        private bool slowMotionEnabled          = false;
        private float slowMotionScaleDefault    = 1.0f;
        private float slowMotionScaleSlow       = 0.5f;

        private Keys keyEnabledSlowMotionKeyBoard                       = Keys.Pause;
        private Keys keyIncreasedSloMotionKeyBoard                      = Keys.PageUp;
        private Keys keyDecreasedSloMotionKeyBoard                      = Keys.PageDown;

        private GamepadButtonFlags keyEnabledSlowMotionXboxController   = GamepadButtonFlags.LeftShoulder;
        private GamepadButtonFlags keyIncreasedSloMotionXboxController  = GamepadButtonFlags.DPadUp;
        private GamepadButtonFlags keyDecreasedSloMotionXboxController  = GamepadButtonFlags.DPadDown;

        private enum typeScale {
            Decreased = 0,
            Increased = 1            
        }
        #endregion

        #region FUNCTIONS
        private void setScaleSlowMotion(float scale, typeScale type)
        {
            if (this.slowMotionEnabled)
            {
                if (type == typeScale.Increased)
                {
                    if (scale <= 10.0f)
                    {
                        if ((this.slowMotionScaleSlow += scale) > 10.0f)
                        {
                            this.slowMotionScaleSlow = 10.0f;
                        }
                        else
                        {
                            this.slowMotionScaleSlow += scale;
                        }
                    }
                }
                else if (type == typeScale.Decreased)
                {
                    if (scale >= 0.5f)
                    {
                        if ((this.slowMotionScaleSlow -= scale) < 0.5f)
                        {
                            this.slowMotionScaleSlow = 0.5f;
                        }
                        else
                        {
                            this.slowMotionScaleSlow -= scale;
                        }
                    }
                }
            }
        }
        #endregion

        #region CONSTRUCTOR
        public GrantThefAuto() {
            // Show to player how is the key to enable/disable
            try
            {
                Log.i(String.Format("Press {0} or Button Left Shoulder to enable the SlowMotion.", util.GetKeyName(keyEnabledSlowMotionKeyBoard)), "");
            }
            catch (NotSupportedException e)
            {
                keyEnabledSlowMotionKeyBoard = Keys.Pause;
                Log.i(String.Format("Press Pause or Button Left Shoulder to enable the SlowMotion."), "");
            }

            this.Interval = 100;
            // This event check if the xbox controller is connected.
            this.Tick += new EventHandler(this.xboxControllerCheckState);

            // This event is to enabled/disabled the SlowMotion
            // on user press the key on keyboard
            this.BindKey(keyEnabledSlowMotionKeyBoard, this.toggleSlowMotion);

            // This event is to enabled/disabled the SlowMotion
            // on user press the key on xboxController
            this.Tick += new EventHandler(this.toggleSlowMotion);

            // This event is to increase the SlowMotion value
            // on user press the key on keyboard
            this.BindKey(keyIncreasedSloMotionKeyBoard, this.IncreasedSlowMotion);

            // This event is to decrease the SlowMotion value
            // on user press the key on keyboard
            this.BindKey(keyDecreasedSloMotionKeyBoard, this.DecreasedSlowMotion);

            // This event is to increase/decrease the SlowMotion value
            // on user press the key on xboxController
            this.Tick += new EventHandler(this.IncreasedDecreasedSlowMotion);

            // This event is the slowMotion Function
            this.Tick += new EventHandler(this.slowMotion);                        
            Wait(0);
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
                // Check if user is in vehicle
                // if true the Button to enable the slowMotion
                // is the RightShoulder else default Button
                if (Player.Character.isInVehicle())
                    keyEnabledSlowMotionXboxController = GamepadButtonFlags.RightShoulder;
                else
                    keyEnabledSlowMotionXboxController = GamepadButtonFlags.LeftShoulder;

                if (this.GamePad.GetState().Gamepad.Buttons.CompareTo(keyEnabledSlowMotionXboxController) == 0)
                {
                    slowMotionEnabled = !slowMotionEnabled;
                }
            }
        }

        private void IncreasedSlowMotion()
        {
            if (this.slowMotionEnabled)
            {
                this.setScaleSlowMotion(0.5f, typeScale.Increased);
            }
        }

        private void DecreasedSlowMotion()
        {
            if (this.slowMotionEnabled)
            {
                this.setScaleSlowMotion(0.5f, typeScale.Decreased);
            }
        }

        private void IncreasedDecreasedSlowMotion(object sender, EventArgs e)
        {
            if (this.IsGamePadConnected)
            {
                if (this.GamePad.GetState().Gamepad.Buttons.CompareTo(keyIncreasedSloMotionXboxController) == 0) 
                {
                    if (slowMotionEnabled)
                    {
                        this.setScaleSlowMotion(0.5f, typeScale.Increased);
                    }
                }
                else if (this.GamePad.GetState().Gamepad.Buttons.CompareTo(keyDecreasedSloMotionXboxController) == 0)
                {
                    if (this.slowMotionEnabled)
                    {
                        this.setScaleSlowMotion(0.5f, typeScale.Decreased);
                    }
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
