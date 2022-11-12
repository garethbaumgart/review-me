using System.Collections.ObjectModel;
namespace ReviewMe.Domain;
public class Review
{
    private Dictionary<Category, Assessment> _assessments = new();
    public ReadOnlyDictionary<Category, Assessment> Assessments => _assessments.AsReadOnly();
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset? CompletedAt { get; private set; }
    public string Name { get; private set; }
    public User Reviewee { get; }
    public User Reviewer { get; }
    public Review(DateTimeOffset createdAt, string name, User reviewee, User reviewer, DateTimeOffset? completedAt = null)
    {
        CreatedAt = createdAt;
        CompletedAt = completedAt;
        Name = name;
        Reviewee = reviewee;
        Reviewer = reviewer;
    }

    public void AddAssessment(Category category, int weighting)
    {
        var assessment = new Assessment(category, weighting);
        if (!_assessments.TryAdd(category, assessment))
            throw new DuplicateAssessmentCategoryException($"Cannot add a duplicate assessment category. Category => {assessment.Category}");
    }

    public void MarkAsCompleted(User user)
    {
        if (CompletedAt is not null)
            throw new ReviewAlreadyCompletedException($"Review has already been completed.");

        if (user != Reviewer)
            throw new ReviewerExpectedException("Only reviewers can mark a review as complete");

        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}