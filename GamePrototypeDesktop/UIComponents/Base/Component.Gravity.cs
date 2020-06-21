using System;

namespace GamePrototypeDesktop.UIComponents.Base
{
    [Flags]
    public enum Gravity
    {
        None = 0, 
        
        Left = 1,
        Right = 2,
        CenterHorizontal = 4,
        FillHorizontal = 8,
        
        Top = 16,
        Bottom = 32,
        CenterVertical = 64,
        FillVertical = 128,
        
        Center = CenterVertical | CenterHorizontal,
        Fill = FillVertical | FillHorizontal
    }

    // public static class GravityExtension
    // {
    //     public static bool HasFlag(this Gravity gravity, Gravity flag)
    //     {
    //         return (gravity & flag) != 0;
    //     }
    // }
}
