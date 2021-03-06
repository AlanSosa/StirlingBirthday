﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shane.Church.StirlingBirthday.Core.Services
{
    public interface ITileUpdateService
    {
        Task UpdateTile(Action callback = null);
        Task SaveUpcomingImages();
    }
}
