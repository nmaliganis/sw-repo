using System;
using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Accounts;

public class ChangePasswordUiModel
{
    [Required(ErrorMessage = "User Id is required")]
    [Editable(true)] public Guid Id { get; set; }

    [MinLength(8, ErrorMessage = "Το ελάχιστο μέγεθος είναι 8 χαρακτήρες")]
    [MaxLength(16, ErrorMessage = "Το μέγιστο μέγεθος είναι 16 χαρακτήρες")]
    [Required(ErrorMessage = "Το Κωδικός Χρήστη είναι υποχρεωτικός")]
    [Display(Name = "Κωδικός Χρήστη")]
    public string Password { get; set; }

    [MinLength(8, ErrorMessage = "Το ελάχιστο μέγεθος είναι 8 χαρακτήρες")]
    [MaxLength(16, ErrorMessage = "Το μέγιστο μέγεθος είναι 16 χαρακτήρες")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Η Επαλήθευση Κωδικού Χρήστη είναι υποχρεωτική")]
    [Compare(nameof(Password), ErrorMessage = "Οι κωδικοί δεν συμφωνούν")]
    [Display(Name = "Επαλήθευση Κωδικού Χρήστη")]
    public string ConfirmPassword { get; set; }

}//Class : ChangePasswordUiModel