namespace ReviewMe.Domain;
public class User
{
    public string Email {get;}
    public string FirstName {get;}
    public string LastName{get;}

    public User(string email, string firstName, string lastName)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}