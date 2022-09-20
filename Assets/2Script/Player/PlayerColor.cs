using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor
{
    private static List<Color> colors = new List<Color>()
    {
        new Color(1f, 0f, 0f),
        new Color(0.1f, 0.1f, 1f),
        new Color(0f, 0.6f, 0f),
        new Color(1f, 0.3f, 0.9f),
        new Color(1f, 0.4f, 0f),
        new Color(1f, 0.9f, 0.1f),
        new Color(0.2f, 0.2f, 0.2f),
        new Color(0.9f, 1f, 1f),
        new Color(0.6f, 0f, 0.6f),
        new Color(0.7f, 0.2f, 0f),
        new Color(0f, 1f, 1f),
        new Color(0.7f, 1f, 0f)
    };

    public static Color GetColor(EPlayerColor p_playerColor) { return colors[(int)p_playerColor]; }
    public static Color Red { get { return colors[(int)EPlayerColor.RED]; } }
    public static Color Blue { get { return colors[(int)EPlayerColor.BLUE]; } }
    public static Color Green { get { return colors[(int)EPlayerColor.GREEN]; } }
    public static Color Pink { get { return colors[(int)EPlayerColor.PINK]; } }
    public static Color Orange { get { return colors[(int)EPlayerColor.ORANGE]; } }
    public static Color Yellow { get { return colors[(int)EPlayerColor.YELLOW]; } }
    public static Color Black { get { return colors[(int)EPlayerColor.BLACK]; } }
    public static Color White { get { return colors[(int)EPlayerColor.WHITE]; } }
    public static Color Purple { get { return colors[(int)EPlayerColor.PURPLE]; } }
    public static Color Brown { get { return colors[(int)EPlayerColor.BROWN]; } }
    public static Color Cyan { get { return colors[(int)EPlayerColor.CYAN]; } }
    public static Color Lime { get { return colors[(int)EPlayerColor.LIME]; } }
}
