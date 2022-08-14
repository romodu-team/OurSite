using System.Collections.Generic;
using OurSite.Core.DTOs.CheckBoxDtos;

namespace OurSite.Core.DTOs.ConsultationRequestDtos;

public class GetConsulationFormDto
{

        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string Expration { get; set; }
        public string CreateDate { get; set; }
        public string LastUpdateDate { get; set; }
        public bool IsRead { get; set; }
        public List<CheckBoxDto?>? ItemSelected { get; set; }
}

