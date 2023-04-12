﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models.ViewModels
{
    public class PageInfo
    {
        public int TotalNumProjects { get; set; }
        public int BurialsPerPage { get; set; }
        public int CurrentPage { get; set; }
        // figure out how many pages we need- we ceiling it so that if its 5.67, it will be 6. 
        public int TotalPages => (int)Math.Ceiling((double)TotalNumProjects / BurialsPerPage);
    }
}
