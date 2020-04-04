using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class PartyViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(280, ErrorMessage = "El nombre es muy largo")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public string Color { get; set; }
        //[Required(ErrorMessage = "Campo Requerido")]
        public IFormFile Logo { get; set; }
    }
}
