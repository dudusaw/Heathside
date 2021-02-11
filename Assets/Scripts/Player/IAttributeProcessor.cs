namespace Heathside.Control
{
    public interface IAttributeProcessor
    {
        /// <summary>
        /// outputs a new value based on input and some behavior.
        /// </summary>
        float Process(float inputValue);
    }
}