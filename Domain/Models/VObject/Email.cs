namespace Domain.Models.VObject
{
    public class Email 
    {
        public string Address { get; set; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException("email cant be empty");
            }
            //could add some regex to cvheck vlaidity

            this.Address = address;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Email email)
            {
                return false;
            }
            return Address == email.Address;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }
    }
}