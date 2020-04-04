using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class voterViewModel
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$",
            ErrorMessage = "Cedula no valida")]
        public string Id { get; set; }
    }
}
