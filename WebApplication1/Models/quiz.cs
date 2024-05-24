using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class quizz
    {
        public int id { get; set; }
        public string question { get; set; }
        public string choix1 { get; set; }
        public string choix2 { get; set; }
        public string choix3 { get; set; }
        public string inputChoix { get; set; }
        public string CheckedChoix { get; set; }
        public string emailUserResponse { get; set; }
        public string emailUserRequest { get; set; }
        public string speciality { get; set; }
        public string photo{ get; set; }

    }
}