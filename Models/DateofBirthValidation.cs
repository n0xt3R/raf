using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateofBirthValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DateTime dob = Convert.ToDateTime(value.ToString());
            if (dob != null)
            {
                DateTime currentDate = DateTime.Now;
                var year = currentDate.Year - 18;

                if (dob.Year > year )
                {
                    return new ValidationResult("Applicant must be 18 years old or older.");
                }
                else
                {
                    return ValidationResult.Success;

                }

            }
            return ValidationResult.Success;

        }
    }
}
