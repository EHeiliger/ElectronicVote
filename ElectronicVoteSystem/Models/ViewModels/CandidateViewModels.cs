using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class CandidateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$",
            ErrorMessage = "Cedula no valida")]
        public string CitizenId { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public int PartyId { get; set; }
        //[Required(ErrorMessage = "Campo Requerido")]
        public IFormFile ProfileAvatar { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public bool Status { get; set; }

    }
}
