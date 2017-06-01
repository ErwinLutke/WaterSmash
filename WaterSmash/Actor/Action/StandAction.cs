namespace Water
{
    internal class StandAction : IAction
    {
        private AActor aActor;

        public StandAction(AActor aActor)
        {
            this.aActor = aActor;
        }
    }
}