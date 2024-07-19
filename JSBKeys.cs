using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace JSBSim.Keys
{
    // JSBSim replaces undescore with space.
    public static class Simulation
    {
        public static string TIME = "Time";

        public static int READ_PORT = 1138;
        public static int WRITE_PORT = 1100;


    }

    public static class Velocities
    {
        /// <summary>
        /// Direction of aircraft nose. Z axis in Unity.
        /// </summary>
        public static string U_FPS = "u-fps";
        /// <summary>
        /// Direction of right wing. X axis in Unity.
        /// </summary>
        public static string V_FPS = "v-fps";
        /// <summary>
        /// Down direction. -Y axis in Unity. (Inverted)
        /// </summary>
        public  static string W_FPS = "w-fps";

        /// <summary>
        /// Roll. -Z axis in Unity. (Inverted)
        /// </summary>
        public static string P_RAD_SEC = "p-rad sec";

        /// <summary>
        /// Pitch. -X axis in Unity. (Inverted)
        /// </summary>
        public static string Q_RAD_SEC = "q-rad sec";

        /// <summary>
        /// Yaw. Y axis in Unity.
        /// </summary>
        public static string R_RAD_SEC = "r-rad sec";

    }

    public static class IC
    {
        // explicitly define a new name using caption= tag to avoid duplicate.
        // e.g <property> ic/h-agl-ft </property> to
        // <property caption= "ic/h-agl-ft"> ic/h-agl-ft </property>

        public static string HEIGHT_ABOVE_GL_FT = "ic/h-agl-ft";


    }



    public static class Propulsion
    {
        public static string R_STARTER_CMD = "starter cmd";
        public static string R_MAGNETO_CMD = "magneto cmd";

        public static string W_STARTER_CMD = "propulsion/starter_cmd";
        public static string W_MAGNETO_CMD = "propulsion/magneto_cmd";

    }

    public static class Position
    {
        public static string R_HEIGHT_ABOVE_GL_FT = "h-agl-ft";
        public static string W_HEIGHT_ABOVE_GL_FT = "position/h-agl-ft";

        public static string R_HEIGHT_ABOVE_GL_KM = "h-agl-km";
        public static string W_HEIGHT_ABOVE_GL_KM = "position/h-agl-km";

        public static string R_HEIGHT_ABOVE_SL_FT = "h-sl-ft";
        public static string W_HEIGHT_ABOVE_SL_FT = "position/h-sl-ft";

        public static string R_HEIGHT_ABOVE_SL_MT = "h-sl-meters";
        public static string W_HEIGHT_ABOVE_SL_MT = "position/h-sl-meters";


    }


    /// <summary>
    /// Flight Control System. Write properties are prefixed with W_ and read with R_.
    /// </summary>
    public static class FCS
    {
        /// <summary>
        /// [0, 1]. 
        /// </summary>
        public static string W_THROTTLE_CMD_NORM = "fcs/throttle-cmd-norm";

        

        /// <summary>
        /// [-1, 1].
        /// </summary>
        public static string W_RUDDER_CMD_NORM = "fcs/rudder-cmd-norm";

        public static string R_RUDDER_POS_NORM = "rudder-pos-norm";
        public static string R_RUDDER_POS_DEG = "rudder-pos-deg";
        public static string R_RUDDER_POS_RAD = "rudder-pos-rad";


        /// <summary>
        /// [-1, 1].
        /// </summary>
        public static string W_ELEVATOR_CMD_NORM = "fcs/elevator-cmd-norm";

        public static string R_ELEVATOR_POS_DEG = "elevator-pos-deg";
        public static string R_ELEVATOR_POS_NORM = "elevator-pos-norm";
        public static string R_ELEVATOR_POS_RAD = "elevator-pos-rad";

        /// <summary>
        /// [-1, 1].
        /// </summary>
        public static string W_AILERON_CMD_NORM = "fcs/aileron-cmd-norm";

        public static string R_LEFT_AILERON_POS_DEG = "left-aileron-pos-deg";
        public static string R_LEFT_AILERON_POS_NORM = "left-aileron-pos-norm";
        public static string R_LEFT_AILERON_POS_RAD = "left-aileron-pos-rad";

        public static string R_RIGHT_AILERON_POS_DEG = "right-aileron-pos-deg";
        public static string R_RIGHT_AILERON_POS_NORM = "right-aileron-pos-norm";
        public static string R_RIGHT_AILERON_POS_RAD = "right-aileron-pos-rad";



    }



}

