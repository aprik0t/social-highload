using System;
using OtusSocial.Dal.Model.Enum;

namespace OtusSocial.Dal.Model.Entity
{
    public class Person
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public string Bio { get; set; }
    }
}