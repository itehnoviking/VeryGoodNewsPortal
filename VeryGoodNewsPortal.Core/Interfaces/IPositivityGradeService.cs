﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeryGoodNewsPortal.Core.Interfaces
{
    public interface IPositivityGradeService
    {
        Task GetAndSavingPositivityGrade();
    }
}
