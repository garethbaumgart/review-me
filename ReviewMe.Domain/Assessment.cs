using System.Collections.ObjectModel;
namespace ReviewMe.Domain;
public class Assessment
{
    public Category Category { get; }
    private Dictionary<User, Feedback> _feedback = new();
    public ReadOnlyDictionary<User, Feedback> Feedback => _feedback.AsReadOnly();
    public int Weighting { get; }
    internal Assessment(Category category, int weighting)
    {
        Category = category;
        Weighting = weighting;
    }

    public void ProvideFeedback(User user, string comments, Rating rating)
    {
        var feedback = new Feedback(user, comments, rating);
        _feedback.Remove(user);
        _feedback.TryAdd(user, feedback);
    }
}