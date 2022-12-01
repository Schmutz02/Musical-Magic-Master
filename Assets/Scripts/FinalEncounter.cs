namespace MMM
{
    public class FinalEncounter : Encounter
    {
        protected override void Start()
        {
            base.Start();
            PassDialog.OnDialogEnd.AddListener(Transition);
        }

        private void Transition()
        {
            SceneTransition.Instance.TransitionToScene("End");
        }
    }
}