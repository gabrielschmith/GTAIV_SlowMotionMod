using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA;

namespace ModSlowMotion
{
    class Log
    {
        public static void e(String sFlag, String sMessage, int Duration = 10000)
        {
            try
            {
                Game.DisplayText(string.Format("Error: {0} - {1}", sFlag, sMessage), Duration);
            }
            catch (Exception ex) { }
        }

        public static void i(String sFlag, String sMessage, int Duration = 10000)
        {
            try
            {
                Game.DisplayText(string.Format("Info: {0} - {1}", sFlag, sMessage), Duration);
            }
            catch (Exception ex) { }
        }
    }
}
