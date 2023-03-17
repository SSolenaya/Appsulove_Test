namespace Assets.Scripts
{
    public interface ITouchable
    {
        void OnWasTouched(ICanTouch canTouch);
    }
}