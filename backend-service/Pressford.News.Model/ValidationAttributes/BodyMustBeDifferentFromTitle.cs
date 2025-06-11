using Pressford.News.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pressford.News.API.ValidationAttributes
{
    public class ArticleBodyMustBeDifferentFromTitleAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is not
            ArticleBase artcile)
            {
                throw new Exception($"Attribute " +
                    $"{nameof(ArticleBodyMustBeDifferentFromTitleAttribute)} " +
                    $"must be applied to a " +
                    $"{nameof(ArticleBase)} or derived type.");
            }

            if (artcile.Title == artcile.Body)
            {
                return new ValidationResult(
                "The provided body should be different from the title.",
                    new[] { nameof(ArticleBase) });
            }

            return ValidationResult.Success;
        }
    }
}
