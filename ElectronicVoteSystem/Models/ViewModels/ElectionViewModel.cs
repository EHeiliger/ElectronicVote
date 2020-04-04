using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicVoteSystem.Models.ViewModels
{
    public class ElectionViewModel
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Campo Requerido")]
        [StringLength(280, ErrorMessage = "El nombre es muy largo")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Date)]
        public DateTime DateInit { get; set; }
        
        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Time)]
        public DateTime InitTime { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Date)]
        [DateEndCompare("DateInit")]
        public DateTime DateEnd { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public int PositionId { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        public bool Status { get; set; }


    }

    public class DateEndCompare : ValidationAttribute
    {
       // private readonly DateTime dateinit;
        private readonly string _comparisonProperty;

        public DateEndCompare(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable))
            {
                throw new ArgumentException("value has not implemented IComparable interface");
            }


            var model = (Models.ViewModels.ElectionViewModel)validationContext.ObjectInstance;
            DateTime dateend = Convert.ToDateTime(value);
            DateTime _dateinit = Convert.ToDateTime(model.DateInit);

            if (dateend < _dateinit)
            {
                return new ValidationResult
                    ("La fecha final no puede ser antes de la fecha de inicio");
            }
        
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}

