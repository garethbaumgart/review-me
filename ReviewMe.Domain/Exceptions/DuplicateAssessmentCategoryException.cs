namespace ReviewMe.Domain;

public class DuplicateAssessmentCategoryException : DomainException
{
    public DuplicateAssessmentCategoryException(string message) : base(message) { }
}