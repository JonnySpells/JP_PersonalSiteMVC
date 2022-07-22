using System.ComponentModel.DataAnnotations;

namespace JP_PersonalSite.Models
{
    public class ContactViewModels
    {
        [Required(ErrorMessage = "* Your name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Your email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "* Email address must be formatted correctly")]
        public string Email { get; set; }

        [Required(ErrorMessage = "* Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "* A message is required")]
        [DataType(DataType.MultilineText)] //Ensures we will generate a multi-line textbox (<textarea>)
        public string Message { get; set; }
    }

}
