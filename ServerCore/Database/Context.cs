﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ServerCore.Database.Entities;

namespace ServerCore.Database
{
    class Context : IdentityDbContext<User>
    {   
        
    }
}
