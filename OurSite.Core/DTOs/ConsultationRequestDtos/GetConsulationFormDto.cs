using System.Collections.Generic;
using OurSite.Core.DTOs.CheckBoxDtos;

namespace OurSite.Core.DTOs.ConsultationRequestDtos;

public class GetConsulationFormDto
{

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public bool IsRead { get; set; }
        public List<CheckBoxDto?>? ItemSelected { get; set; }
}

