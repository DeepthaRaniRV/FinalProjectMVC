//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABCInternationalSchool.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public System.DateTime DOB { get; set; }
        public int Age { get; set; }
        public int Branch_id { get; set; }
        public int Class_id { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual Processed_applications Processed_applications { get; set; }
    }
}
