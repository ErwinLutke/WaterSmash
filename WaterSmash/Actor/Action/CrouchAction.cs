namespace Water
{
    internal class CrouchAction : IAction
    {
        private AActor aActor;

        public CrouchAction(AActor aActor)
        {
            this.aActor = aActor;
        }
    }
}