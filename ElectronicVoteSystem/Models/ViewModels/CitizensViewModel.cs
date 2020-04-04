using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class CitizensViewModel
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$",
         ErrorMessage = "Cedula no valida")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public bool Status { get; set; }
    }
}
