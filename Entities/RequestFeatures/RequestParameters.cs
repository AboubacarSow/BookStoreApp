﻿namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        //Auto-implemented property
        public int PageNumber { get; set; }
        //Full Property
        private int _pageSize;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }
        public String? OrderBy { get; set; }
        public String? Fields { get; set; }
    }
}
