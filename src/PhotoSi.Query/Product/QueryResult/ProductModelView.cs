﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSi.Query.Product.QueryResult
{
    public class ProductModelView
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Dictionary<int, string> Options { get; set; }
    }
}
