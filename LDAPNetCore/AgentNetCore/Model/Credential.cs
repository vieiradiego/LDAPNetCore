namespace AgentNetCore.Model
{
    public class Credential
    {
        private Credential _credential;
        public Credential(Credential credential)
        {
            _credential = credential;
        }
        public long? Id { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string Domain { get; set; }

    }
}
