using Pressford.News.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pressford.News.Model
{
	//[ArticleBodyMustBeDifferentFromTitle] //Not using but left for reference
	public class ArticleBase : IValidatableObject
	{
		[Required(ErrorMessage = "Article Name is required.")]
		//[StringLength(20, MinimumLength = 5, ErrorMessage = "Article Name must be between 5 and 10 characters long.")]
		//[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
		public string Title { get; set; }


		[Required(ErrorMessage = "Article Body is required.")]
		public string Body { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
            if(Title == Body)
			{
				yield return new ValidationResult("The Article body should be different from the title.",
					new[] { "Article" });
			}
		}
	}

	public class ReadArticle : ArticleBase
	{
		public int ArticleId { get; set; }
		public string Author { get; set; }
		public DateTime DatePublished { get; set; }
		public DateTime DateModified { get; set; }
	}

	public class UpdateArticle : ArticleBase
	{
		public int ArticleId { get; set; }
	}

	//todo think about better model design here is an example
	/* Common DTOs
	  public class CourseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid AuthorId { get; set; }
    }

	// Common but differen name for easy understaning
	    public class CourseForCreationDto : CourseForManipulationDto
    {     }

	 public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a title.")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters.")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "The description shouldn't have more than 1500 characters.")]
        public virtual string Description { get; set; } //virtual to allow override in derived classes
    }

	   public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description.")] //Override only validation still using  other bits
        public override string Description { get => base.Description; set => base.Description = value; }

    }


	 * */

}