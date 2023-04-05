﻿using System;
using System.Collections.Generic;

namespace ESM.Modules.DataAccess.Models;

public partial class Laptop : ProductDTO
{

    public string Cpu { get; set; } = null!;

    public string Ram { get; set; } = null!;

    public string Storage { get; set; } = null!;

    public string Graphic { get; set; } = null!;


    public string? Series { get; set; }

    public string? Need { get; set; }
}
