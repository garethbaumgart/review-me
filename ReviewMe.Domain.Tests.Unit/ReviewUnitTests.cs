using AutoFixture;
using FluentAssertions;

namespace ReviewMe.Domain.Tests.Unit;

public class ReviewUnitTests
{
    private Fixture Fixture = new Fixture();

    [Fact]
    public void GivenAReview_InvokingMarkAsComplete_SetsCompletedAd()
    {
        // Arrange
        var reviewer = Fixture.Create<User>();
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", Fixture.Create<User>(), reviewer);

        // Act
        review.MarkAsCompleted(review.Reviewer);

        // Assert
        review.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public void GivenACompletedReview_InvokingMarkAsComplete_ThrowsAnException()
    {
        // Arrange
        var review = Fixture.Create<Review>();

        // Act
        var exception = Record.Exception(() => review.MarkAsCompleted(review.Reviewer));

        // Assert
        exception.Should().BeOfType<ReviewAlreadyCompletedException>();
    }

    [Fact]
    public void GivenAReivew_InvokeingMarkAsCompleteAsANonReviewer_ThrowsAnException()
    {
        // Arrange
        var reviewer = Fixture.Create<User>();
        var reviewee = Fixture.Create<User>();
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", Fixture.Create<User>(), Fixture.Create<User>());

        // Act
        var exception = Record.Exception(() => review.MarkAsCompleted(reviewee));

        // Assert
        exception.Should().BeOfType<ReviewerExpectedException>();
    }

    [Fact]
    public void GivenAReview_AddingAnAssessment_AddsAnAssessment()
    {
        // Arrange
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", Fixture.Create<User>(), Fixture.Create<User>());
        var category = Fixture.Create<Category>();

        // Act
        review.AddAssessment(category, 10);

        // Assert
        review.Assessments.Count().Should().Be(1);
        var assessment = review.Assessments.First();
        assessment.Key.Should().Be(category);
        assessment.Value.Category.Should().Be(category);
        assessment.Value.Feedback.Should().BeEmpty();
    }

    [Fact]
    public void GivenAReviewWithAssessments_AddingAnAssessmentWithADuplicateCategory_ThrowsAnException()
    {
        // Arrange
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", Fixture.Create<User>(), Fixture.Create<User>());
        var category = new Category("Test Cat", "Hello Test Categoryu");
        review.AddAssessment(category, 30);

        // Act
        var exception = Record.Exception(() => review.AddAssessment(category, 50));

        // Assert
        review.Assessments.Count().Should().Be(1);
        exception.Should().BeOfType<DuplicateAssessmentCategoryException>();
    }

    [Fact]
    public void GivenAReviewWithAssessments_ReviewerProvidingFeedback_AddReviewerFeedback()
    {
        // Arrange
        var reviewer = Fixture.Create<User>();
        var comments = "Some feedback given here..";
        var rating = Rating.RoleModel;
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", Fixture.Create<User>(), reviewer);
        var category = Fixture.Create<Category>();
        review.AddAssessment(category, 25);

        var assessment = review.Assessments[category];

        // Act
        assessment.ProvideFeedback(reviewer, comments, rating);

        // Assert
        assessment.Feedback.Should().HaveCount(1);
        assessment.Feedback.Should().ContainKey(reviewer);

        var feedback = assessment.Feedback[reviewer];
        feedback.Assessor.Should().Be(reviewer);
        feedback.Comments.Should().Be(comments);
        feedback.Rating.Should().Be(rating);
    }

    [Fact]
    public void GivenAReviewWithAssessments_RevieweeProvidingFeedback_AddRevieweesFeedback()
    {
        // Arrange
        var reviewee = Fixture.Create<User>();
        var comments = "Some feedback from the reviewee given here..";
        var rating = Rating.RoleModel;
        var review = new Review(DateTimeOffset.UtcNow, "Test Review", reviewee, Fixture.Create<User>());
        var category = Fixture.Create<Category>();
        review.AddAssessment(category, 25);

        var assessment = review.Assessments[category];

        // Act
        assessment.ProvideFeedback(reviewee, comments, rating);

        // Assert
        assessment.Feedback.Should().HaveCount(1);
        assessment.Feedback.Should().ContainKey(reviewee);

        var feedback = assessment.Feedback[reviewee];
        feedback.Assessor.Should().Be(reviewee);
        feedback.Comments.Should().Be(comments);
        feedback.Rating.Should().Be(rating);
    }

    [Fact]
    public void GivenAReviewWithAssessments_AddingAReviewTwice_OnlyKeepsTheLastAddedReview()
    {
        // Arrange
        var reviewer = Fixture.Create<User>();
        var review = new Review(DateTimeOffset.UtcNow, "Test review", Fixture.Create<User>(), reviewer);
        var category = Fixture.Create<Category>();
        review.AddAssessment(category, 30);

        var assessment = review.Assessments[category];
        var feedbackOneComments = "ONE";
        var feedbackOneRating = Rating.Unsatisfactory;
        assessment.ProvideFeedback(reviewer, feedbackOneComments, feedbackOneRating);

        var feedbackTwoComments = "TWO";
        var feedbackTwoRating = Rating.Expected;

        // Act
        assessment.ProvideFeedback(reviewer, feedbackTwoComments, feedbackTwoRating);

        // Assert
        assessment.Feedback.Should().HaveCount(1);
        assessment.Feedback.Should().ContainKey(reviewer);
        var feedback = assessment.Feedback[reviewer];
        feedback.Assessor.Should().Be(reviewer);
        feedback.Comments.Should().Be(feedbackTwoComments);
        feedback.Rating.Should().Be(feedbackTwoRating);
    }
}