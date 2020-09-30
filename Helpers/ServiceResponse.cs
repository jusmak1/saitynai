﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialAPI.Helpers
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool Success { get; set; } = true;

        public string Message { get; set; } = null;

        public ResponseType? ResponseType { get; set; } = null;
    }

    public enum ResponseType
    {
        Success,
        NotFound,
        BadRequest
    }
}
