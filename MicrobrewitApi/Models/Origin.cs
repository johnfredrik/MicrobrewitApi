﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models
{
    public class Origin
    {
     
        public int OriginId { get; set; }
        public string Name { get; set; }
    }
}