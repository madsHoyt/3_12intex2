using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intex2.Models
{
    public class WrappingInputData
    {
        public float Depth { get; set; }
        public float Length { get; set; }
        public bool Headdirection_W { get; set; }
        public bool Facebundles_Y { get; set; }
        public bool Goods_Y { get; set; }
        public bool Haircolor_B { get; set; }
        public bool Haircolor_D { get; set; }
        public bool Haircolor_K { get; set; }
        public bool Haircolor_R { get; set; }
        public bool Haircolor_unknown { get; set; }
        public bool Samplescollected_true { get; set; }
        public bool Ageatdeath_C { get; set; }
        public bool Ageatdeath_unknown { get; set; }
    }
}