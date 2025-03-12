namespace Domain.Models.ValueObject
{
    public class Email 
    {
        public string address { get; set; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException("email cant be empty");
            }
            //could add some regex to cvheck vlaidity

            this.address = address;
        }
    }
}