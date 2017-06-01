namespace Water
{
    internal class ThrowAction : IAction
    {
        private AActor aActor;

        public ThrowAction(AActor aActor)
        {
            this.aActor = aActor;
        }
    }
}