using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModulosTaller.Models;

namespace ModulosTaller.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly TallerMotosDbContext _context;

        public ProveedoresController(TallerMotosDbContext context)
        {
            _context = context;
        }
    }
}
