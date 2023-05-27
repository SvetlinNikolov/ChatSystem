using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    // Add additional fields as needed
}
