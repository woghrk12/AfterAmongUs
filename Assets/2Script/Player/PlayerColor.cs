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
        new Color(1f, 0.9f, 0.1f),
        new Color(1f, 0.6f, 0f),
        new Color(0.6f, 0f, 1f),
        new Color(0f, 1f, 1f),
        new Color(0.6f, 0.3f, 1f),
        new Color(1f, 0f, 0.6f),
        new Color(0.9f, 0.9f, 0.9f),
        new Color(0.1f, 0.1f, 0.1f),
        new Color(0.5f, 0.5f, 0.5f)
    };

    public static Color GetColor(EPlayerColor playerColor) { return colors[(int)playerColor]; }
    public static Color Red { get { return colors[(int)EPlayerColor.RED]; } }
    public static Color Blue { get { return colors[(int)EPlayerColor.BLUE]; } }
    public static Color Green { get { return colors[(int)EPlayerColor.GREEN]; } }
    public static Color Yellow { get { return colors[(int)EPlayerColor.YELLOW]; } }
    public static Color Orange { get { return colors[(int)EPlayerColor.ORANGE]; } }
    public static Color Purple { get { return colors[(int)EPlayerColor.PURPLE]; } }
    public static Color Cyan { get { return colors[(int)EPlayerColor.CYAN]; } }
    public static Color Brown { get { return colors[(int)EPlayerColor.BROWN]; } }
    public static Color Pink { get { return colors[(int)EPlayerColor.PINK]; } }
    public static Color White { get { return colors[(int)EPlayerColor.WHITE]; } }
    public static Color Black { get { return colors[(int)EPlayerColor.BLACK]; } }
    public static Color Gray { get { return colors[(int)EPlayerColor.GRAY]; } }
}
