using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class PositionViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(280, ErrorMessage = "El nombre es muy largo")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
