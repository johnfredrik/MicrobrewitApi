﻿using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IOtherRepository
    {
        IList<Other> GetOthers();
        Other GetOther(int id);
    }
}