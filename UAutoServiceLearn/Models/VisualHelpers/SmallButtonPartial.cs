﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UAutoServiceLearn.Models
{
    public class SmallButtonPartial
    {
        public string ButtonStyleType { get; set; }
        public string ActionName { get; set; }
        public string GlyphIcon { get; set; }
        public string Text { get; set; }

        //public int? ServiceId { get; set; }
        public string ServiceId { get; set; }

        public string ActionParameters
        {
            get
            {
                var param = "";
                if(!string.IsNullOrEmpty(ServiceId))
                {
                    param= @"/" + $"{ServiceId}";
                }
                return param;
            }
        }
    }
}
