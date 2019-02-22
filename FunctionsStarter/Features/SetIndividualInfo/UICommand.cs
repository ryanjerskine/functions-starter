namespace FunctionsStarter.Features.SetIndividualInfo
{
    public class UICommand
    {
        public string IndividualsName { get; set; }
        public string IndividualsEmail { get; set; }

        public Command ToCommand()
        {
            return new Command(this.IndividualsName, this.IndividualsEmail);
        }
    }
}