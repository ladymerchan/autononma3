﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace autonoma3Tiempo.Models
{
    public class registroTem
    {
        [Key]
        public int id_registroTem { get; set; }
        public string pais { get; set; }
        public string ciudad { get; set; }
        public string fecha { get; set; }
        public int temperatura_maxima { get; set; }
        public int temperatura_minima { get; set; }
      
    }
    public class registroTemDbContext : DbContext
    {
        public registroTemDbContext(DbContextOptions<registroTemDbContext> options) : base(options)
        {
        }
        public DbSet<registroTem> registroTem { get; set; }
    }
}