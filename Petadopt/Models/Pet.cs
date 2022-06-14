using System;
using System.Collections.Generic;

namespace Petadopt.Models
{
    public partial class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Coat { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
    }
}
