namespace Assets.Scripts
{
    public class WarEntity : GameBehaviour
    {
        public WarFactory OriginFactory { get; set; }

        public override void Recycle()
        {
            OriginFactory.Recycle(this);
        }
    }
}
