namespace ReviewMe.Domain;
public class Feedback
{
    public User Assessor { get; }
    public string Comments { get; }
    public Rating Rating { get; }
    internal Feedback(User assessor, string comments, Rating rating)
    {
        Assessor = assessor;
        Comments = comments;
        Rating = rating;
    }
}