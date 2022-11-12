namespace ReviewMe.Domain;

public class ReviewerExpectedException : DomainException
{
    public ReviewerExpectedException(string message) : base(message) { }
}