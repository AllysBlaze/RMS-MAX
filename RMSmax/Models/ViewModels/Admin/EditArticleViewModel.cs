﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditArticleViewModel : MainViewModel
    {
        public Article Article { get; set; }
        public IFormFile PhotoIn { set; get; }
        public IFormFile PhotoCover { set; get; }
    }
}
