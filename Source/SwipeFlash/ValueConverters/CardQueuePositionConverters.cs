using System.Windows;

namespace SwipeFlash
{
    /// <summary>
    /// A static converter that takes in a card queue position and returns a <see cref="Thickness"/>
    /// </summary>
    public static class CardQueuePositionToMarginConverter
    {
        public static Thickness Convert(int value)
        {
            int leftMarginOffset = -5;
            int topMarginOffset = -10;

            return new Thickness((int)value * leftMarginOffset, (int)value * topMarginOffset, -(int)value * leftMarginOffset, -(int)value * topMarginOffset);
        }
    }

    /// <summary>
    /// A static converter that takes in a card queue position to an opacity
    /// </summary>
    public static class CardQueuePositionToOpacityConverter
    {
        public static double Convert(int value)
        {
            switch (value)
            {
                case 0:
                    return 1;
                case 1:
                    return 1;
                case 2:
                    return 0.5;
                case 3:
                    return 0.25;
                default:
                    return 0;
            }
        }
    }
}
