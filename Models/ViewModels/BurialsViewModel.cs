using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models.ViewModels
{
    public class BurialsViewModel
    {
        public IQueryable<BestFinalMerged> BestFinalMergeds { get; set; }
        public PageInfo PageInfo { get; set; }

    }
}
