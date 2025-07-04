using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pressford.News.Model
{
    // Base class remains pure structure
    //[ArticleBodyMustBeDifferentFromTitle] //Not using but left for reference
    public abstract class ArticleBase : IValidatableObject
    {
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Body)
            {
                yield return new ValidationResult("The Article body should be different from the title.",
                    new[] { "Article" });
            }
        }
    }

    // Creation defines complete validation rules
    public class CreateArticle : ArticleBase
    {
        [Required(ErrorMessage = "Article Title is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "{0} must be between {2} and {1} characters long.")]
        public override string Title { get; set; }

        [Required(ErrorMessage = "Article Body is required")]
        public override string Body { get; set; }

    }

    // UpdateArticle inherits all validation from CreateArticle automatically
    public class UpdateArticle : CreateArticle
    {
        [Required(ErrorMessage = "Article ID is required")]
        public int ArticleId { get; set; }

        // Inherits Required validation from CreateArticle
    }

    // PATCH operation chooses its own validation strategy
    public class PatchArticle : ArticleBase
    {
        [Required(ErrorMessage = "Article ID is required")]
        public int ArticleId { get; set; }

        //this will be only applied if passed since its not mandatory
        [StringLength(100, MinimumLength = 5)]
        public override string Title { get; set; }

        [StringLength(2000, MinimumLength = 10)]
        public override string Body { get; set; }
    }

    public class ReadArticle : ArticleBase
    {
        public int ArticleId { get; set; }
        public string Author { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime DateModified { get; set; }

        //Added these 2 prop Age and TitleWithBody to set advanced sorting ideally this sud 
        // be part of author but we do not have endpoints for it.
        public int Age { get; set; }
        public string TitleWithBody { get; set; }
    }
}
