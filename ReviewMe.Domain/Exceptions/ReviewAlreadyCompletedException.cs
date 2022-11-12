namespace ReviewMe.Domain;
public class ReviewAlreadyCompletedException : DomainException
{
    public ReviewAlreadyCompletedException(string message) : base(message) { }
}