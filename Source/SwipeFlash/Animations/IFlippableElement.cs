namespace SwipeFlash
{
    /// <summary>
    /// An interface necessary for the addition of a flip animation
    /// </summary>
    public interface IFlippableElement
    {
        /// <summary>
        /// The duration of the flip animation
        /// </summary>
        float FlipAnimDuration { get; }
    }
}
