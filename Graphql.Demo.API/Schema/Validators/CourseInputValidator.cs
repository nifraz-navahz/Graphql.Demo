using FluentValidation;
using Graphql.Demo.API.Schema.Mutations;

namespace Graphql.Demo.API.Schema.Validators
{
    public class CourseInputValidator : AbstractValidator<CourseInputType>
    {
        public CourseInputValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(5)
                .WithMessage("The course name must contain minimum 5 characters.")
                .MaximumLength(20)
                .WithMessage("The course name must contain maximum 20 characters.")
                //.Must(x => x.All(char.IsUpper))
                .Must(x => x.Any() && char.IsUpper(x[0]))
                .WithErrorCode("UpperCaseValidator")
                .WithMessage("The course name must start with an upper case character.")
                ;
        }
    }
}
